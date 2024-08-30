import axios from 'axios';
import { BASE_URL } from '../settings';

const login = async (email, password) => {
    const response = await axios.post(`${BASE_URL}/auth/login`, { email, password });
    if (response.data.token) {
        localStorage.setItem('user', JSON.stringify(response.data));
    }
    return response.data;
};

const register = async (email, password) => {
    const response = await axios.post(`${BASE_URL}/auth/register`, { email, password });
    return response.data;
};

const logout = () => {
    localStorage.removeItem('user');
};

const getCurrentUser = () => {
    return JSON.parse(localStorage.getItem('user'));
};

export default {
    login,
    register,
    logout,
    getCurrentUser,
};