import axios from 'axios';

export const BASE_URL = 'https://192.168.1.15:7286';

export const axiosInstance = axios.create({
  baseURL: BASE_URL,
  httpsAgent: new (require('https').Agent)({
    rejectUnauthorized: false, // Disable SSL verification (for development only)
  }),
});