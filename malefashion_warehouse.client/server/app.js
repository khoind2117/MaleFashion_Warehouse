/*******************
 * Node.js server
********************/
const config = require('./config');
const express = require('express');
const axios = require('axios');
const router = express.Router();
const compression = require('compression');
const cors = require('cors');
const passport = require('passport');
const LocalStrategy = require('passport-local');

const bodyParser = require('body-parser');
const session = require('express-session');
const {createProxyMiddleware} = require('http-proxy-middleware');

/***********************
 * SETUP EXPRESS SERVER
***********************/

const app = express();
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(compression());
app.set('trust proxy', 1);

(async () => {
    /********
     * CORS
     ********/
    app.use(cors({
        origin: true,
        methods: ['GET', 'POST', 'PUT', 'DELETE', 'OPTIONS', 'PATCH'],
        allowedHeaders: ['Content-Type', 'Authorization'],
        exposedHeaders: ['Content-Type'],
        credentials: true,
    }));

// SETUP SESSION
    const KEY_MFW_client = 'MFW-client';
    app.use(session({
        secret: config.cookieSecret,
        name: KEY_MFW_client,
        proxy: true,
        resave: false,
        saveUninitialized: false,
        cookie: {
            httpOnly: true,
            secure: process.env.NODE_ENV === 'production',
            sameSite: process.env.NODE_ENV === 'production' ? 'none' : 'lax'
        }
    }))

// PASSPORT and SESSION
    app.use(passport.initialize());
    app.use(passport.session());

    app.use('/', router);

// Serialize / Deserialize
    passport.serializeUser(function (user, done) {
        done(null, user);
    });

    passport.deserializeUser(function (user, done) {
        done(null, user);
    });

    app.listen(config.appPort, () => {
        console.log(`App listening at http://localhost:${config.appPort}`);
    });

// LOGIN WITH PASSPORT
    router.post(
        '/',
        passport.authenticate('local', {
            successRedirect: '../dashboard',
            failureRedirect: '/auth/login',
        })
    );

//  STRATEGY LOCAL
    passport.use(
        new LocalStrategy(function (username, password, done) {
            return done(null, username);
        })
    );

    const auth = () => {
        return (req, res, next) => {
            passport.authenticate('local', async (error, user, info) => {
                if (error) {
                    res.status(400).json({statusCode: 200, message: error});
                }

                try {
                    const resAuth = await axios({
                        method: 'post',
                        url: config.apiEndpoint + '/auth/login',
                        data: req.body,
                    });

                    if (resAuth.data?.status === 200) {
                        const user = {
                            accessToken: resAuth.data.data.accessToken,
                            expiresIn: resAuth.data.data.expiresIn,
                            refreshToken: resAuth.data.data.refreshToken,
                        };

                        req.login(user, (err) => {
                            if (err) {
                                return res.status(500).json({ statusCode: 500, message: 'Login failed', error: err });
                            }

                            req.headers['access-token'] = `${user.accessToken}`;
                            return next();
                        });
                    } else {
                        return res.status(400).json('catch:', resAuth.data);
                    }
                } catch (err) {
                    return res.status(400).json(err.response.data);
                }
            })(req, res, next);
        };
    };

    app.post('/authenticate', auth(), (req, res) => {
        res.status(200).json({ statusCode: 200, user: req.user });
    });

    const isLoggedIn = (req, res, next) => {
        if (req.isAuthenticated()) {
            return next();
        }
        return res.status(401).json({statusCode: 401, message: 'not authenticated'});
    };

    app.get('/secure', isLoggedIn, (req, res) => {
        res.status(200).json({ statusCode: 200, user: req.session.passport.user });
    });

    app.get('/status', isLoggedIn, (req, res) => {
        res.status(200).json({ statusCode: 200, isAuthentication: true });
    });

/**********************
 * SETUP SERVICE PROXY
**********************/
    if (config.proxies) {
        const simpleRequestLogger = (proxyServer, options) => {
            proxyServer.on('proxyReq', (proxyReq, req, res) => {
                console.log(`[HPM] [${req.method}] ${req.url}`);
            });
        };

        // API PROXY OPTIONS
        const apiProxyOptions = {
            target: '',
            pathRewrite: {},
            changeOrigin: true,
            logLevel: 'debug',
            proxyTimeout: 30000,
            timeout: 30000,
            plugins: [
                simpleRequestLogger,
            ],
            on: {
                proxyReq: (proxyReq, req, res) => {
                    if ((req.method === 'POST' || req.method === 'PUT') && req.body) {
                        const contentType = req.headers['content-type'] || '';
                        if (!contentType.includes('multipart/form-data')) {
                            try {
                                const newBody = JSON.stringify(req.body);
                                proxyReq.setHeader('Content-Length', Buffer.byteLength(newBody, 'utf8'));
                                proxyReq.write(newBody);
                                proxyReq.end();
                            } catch (e) {
                                console.error('[ProxyReq] JSON stringify error:', e);
                            }
                        }
                    }
                }
            },
            error: (err, req, res) => {
                console.error('Proxy Error:', err);
                res.status(502).json({ error: 'Proxy connection failed' });
            },
        };

        // MIDDLEWARE API
        router.use('/api', handlerApiRoute);

        function handlerApiRoute(req, res, next) {
            if (!req.isAuthenticated()) {
                console.log('is not Authenticated!');
                return next();
            }

            const expiresTimestamp = Date.parse(req.session.passport.user.expiresIn);
            const currentTimestamp = Date.now();
            const isExpiredToken = currentTimestamp >= expiresTimestamp;
            if (isExpiredToken) {
                console.log('token_timeout');
                axios({
                    method: 'post',
                    url: config.apiEndpoint + '/auth/refresh-token',
                    headers: {
                        'access-token': req.session.passport.user.accessToken,
                        'refresh-token': req.session.passport.user.refreshToken,
                    },
                })
                    .then(function (resAuth) {
                        if (resAuth.data?.status === 200) {
                            const user = {
                                accessToken: resAuth.data.data.accessToken,
                                refreshToken: resAuth.data.data.refreshToken,
                                expiresIn: resAuth.data.data.expiresIn,
                            };
                            req.login(user, function (error) {
                                req.headers['access-token'] = `${user.accessToken}`;
                                next();
                            });
                        } else {
                            req.session.destroy();
                            delete req.sessionID;
                            res.status(400).json({ status: 400, message: 'TOKEN_ERROR' });
                        }
                    })
                    .catch(function (err) {
                        req.session.destroy();
                        delete req.sessionID;
                        res.status(400).json({ status: 400, message: 'TOKEN_ERROR' });
                    });
            } else {
                req.headers['access-token'] = `${req.session.passport.user.accessToken}`;
                req.headers['refresh-token'] = `${req.session.passport.user.refreshToken}`;
                next();
            }
        }

        // PROXY MIDDLEWARE
        Object.keys(config.proxies).forEach((proxyKey) => {
            const proxySetting = config.proxies[proxyKey];
            const proxyPathRewrite = proxySetting.pathRewrite;
            const proxyOptions = {
                ...apiProxyOptions,
                target: proxySetting.url,
                pathRewrite: { [`^${proxySetting.pathRewrite}`]: '' },
            };

            router.use(proxyPathRewrite, createProxyMiddleware(proxyOptions));

            console.log(`[HPM] Proxy created: ${proxySetting.pathRewrite} -> ${proxySetting.url}`);
            // TODO: Make the proxy rewrite rule dynamic
            console.log(`[HPM] Proxy rewrite rule created: "^${proxySetting.pathRewrite}" ~> ""`);
        });
    }

    app.post('/logout', (req, res) => {
        console.log('logout');
        res.clearCookie('MFW_client');

        req.logout(function(err) {
            if (err) {
                console.error('Logout error:', err);
                return res.status(500).json({ statusCode: 500, message: 'Logout failed', error: err });
            }
            req.session.destroy(function(err) {
                if (err) {
                    console.error('Session destroy error:', err);
                    return res.status(500).json({ statusCode: 500, message: 'Logout failed', error: err });
                }
                delete req.sessionID;
                return res.status(200).json({ statusCode: 200, message: 'Logout success' });
            });
        });
    });
})();

module.exports = app;
