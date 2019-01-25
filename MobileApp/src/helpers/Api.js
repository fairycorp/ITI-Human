import Axios from 'axios';

class API {

    get Path() { return process.env.BACKEND_URL1 }

    get(endpoint) {
        const checked = this.routineCheck(endpoint);
        let url = `${this.Path}${checked}`;
        return Axios.get(url);
    }

    post(endpoint, data) {
        const checked = this.routineCheck(endpoint);
        let url = `${this.Path}${checked}`;
        return Axios.post(url, data);
    }

    put(endpoint, data) {
        const checked = this.routineCheck(endpoint);
        let url = `${this.Path}${checked}`;
        return Axios.put(url, data);
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