const settings = {};

settings.node_env = process.env.NODE_ENV || 'development';
process.env['NODE_TLS_REJECT_UNAUTHORIZED'] = '0';

settings.cookieSecret = process.env.cookieSecret || 'YOUR_COOKIE_SECRET';

settings.appUrl = process.env.APP_URL || 'http://localhost';
settings.appPort = process.env.PORT || '80';
settings.apiEndpoint = process.env.API_ENDPOINT || 'http://localhost:5219/api';
settings.allowOrigin = process.env.ALLOW_ORIGIN || settings.appUrl;

settings.proxies = {
    node: {
        url: settings.appUrl,
        pathRewrite: '/api/node',
    },
    auth: {
        url: settings.apiEndpoint + '/auth',
        pathRewrite: '/api/auth',
    },
    account: {
        url: settings.apiEndpoint + '/account',
        pathRewrite: '/api/account',
    }
}

module.exports = settings;
