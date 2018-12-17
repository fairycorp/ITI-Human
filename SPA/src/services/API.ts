import Axios, { AxiosRequestConfig } from "axios";

class API {
    private configuration: any;
    private requestConfig: AxiosRequestConfig;

    get Path() { return `http://${this.configuration.host}:${this.configuration.port}/`; }

    constructor() {
        this.configuration = {
            host: "localhost",
            port: 5000,
        };
        this.requestConfig = {
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Authorization": "Bearer CfDJ8I4393VoVCJBgJ_CPZXSn1o3WiS0NvnEYDEhfcjBvBEmTreyUPEa3DnR-oorFDRsOk-iNch1x4BaK7hkzEMonb1FSnGr0I63pX9zpFaMojtdnleE9Lq__hH1DudN8hRvjDuSWVt001YLJJOGHS0jzWHLIpidfbtMu726NDPsrDo3mP2lc5gv9iOSXAc5nm-Waw"
            },
        };
    }

    public get(endpoint: string) {
        const checked = this.routineCheck(endpoint);
        return Axios.get(`${this.Path}${checked}`, this.requestConfig);
    }

    public post(endpoint: string, data: any) {
        const checked = this.routineCheck(endpoint);
        return Axios.post(`${this.Path}${checked}`, data, this.requestConfig);
    }

    public put(endpoint: string, data: any) {
        const checked = this.routineCheck(endpoint);
        return Axios.put(`${this.Path}${checked}`, data, this.requestConfig);
    }

    public delete(endpoint: string) {
        const checked = this.routineCheck(endpoint);
        return Axios.delete(`${this.Path}${checked}`, this.requestConfig);
    }

    // Checks if endpoint starts with a "/" and removes it if it"s the case.
    private routineCheck(endpoint: string) {
        return (endpoint.startsWith("/")) ? endpoint.substr(1) : endpoint;
    }

}
export default new API();
