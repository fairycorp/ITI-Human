import Axios from 'axios';

class API {
    private configuration: any;

    get Path() { return `http://${this.configuration.host}:${this.configuration.port}/`; }

    constructor() {
        this.configuration = {
            host: 'localhost',
            port: 5000,
        };
    }

    public get(endpoint: string) {
        const checked = this.routineCheck(endpoint);
        return Axios.get(`${this.Path}${checked}`);
    }

    public post(endpoint: string, data: any) {
        const checked = this.routineCheck(endpoint);
        return Axios.post(`${this.Path}${checked}`, data);
    }

    public put(endpoint: string, data: any) {
        const checked = this.routineCheck(endpoint);
        return Axios.put(`${this.Path}${checked}`, data);
    }

    public delete(endpoint: string) {
        const checked = this.routineCheck(endpoint);
        return Axios.delete(`${this.Path}${checked}`);
    }

    // Checks if endpoint starts with a '/' and removes it if it's the case.
    private routineCheck(endpoint: string) {
        return (endpoint.startsWith('/')) ? endpoint.substr(1) : endpoint;
    }

}
export default new API();
