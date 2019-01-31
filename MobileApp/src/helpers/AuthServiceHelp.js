import {AuthService} from "@signature/webfrontauth";
import axios from 'axios';
let instance;
export async function initialize() {
    const config = {
        identityEndPoint: {
          hostname: process.env.HOST_NAME,
          port: 5000,
          disableSsl: true
        }
      };
    instance = await AuthService.createAsync(config, axios );
    return instance;
}
export function getAuthService() { return instance; }