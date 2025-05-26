import {MapRouting} from "@common-lib";

const AUTH = 'auth';
const DASHBOARD = 'dashboard';

const mergeRouting = (...args: any[]): string => {
    return args.join('/');
}

export const ROUTING_MAPPING = {
    auth: {
        root: new MapRouting(AUTH, null),
        login: new MapRouting(AUTH, 'login'),
        register: new MapRouting(AUTH, 'register'),
    },
    dashboard: {
        root: new MapRouting(DASHBOARD, null),
    },
}
