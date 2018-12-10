import Axios from 'axios';

class API {
    
    constructor() {
    }

    //getHost () { return "10.8.110.166"; }

    //getPort () { return 5000; }

    get Path() { return process.env.BACKEND_URL }

    get(endpoint) {
        const checked = this.routineCheck(endpoint);
        let url = `${this.Path}${checked}`;
        console.log('url: ' + url);
        return Axios.get(url);
    }

    post(endpoint, data) {
        const checked = this.routineCheck(endpoint);
        return Axios.post(`${this.Path}${checked}`, data);
    }

    put(endpoint, data) {
        const checked = this.routineCheck(endpoint);
        return Axios.put(`${this.Path}${checked}`, data);
    }

    deleteT(endpoint) {
        const checked = this.routineCheck(endpoint);
        return Axios.delete(`${this.Path}${checked}`);
    }

    // Checks if endpoint starts with a '/' and removes it if it's the case.
    routineCheck(endpoint) {
        return (endpoint.startsWith('/')) ? endpoint.substr(1) : endpoint;
    }
}
export default new API();