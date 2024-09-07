import axios from 'axios';
import { BASE_URL } from '../settings';

const login = async (email, password) => {
    const response = await axios.post(`${BASE_URL}/auth/login`, { email, password });
   localStorage.setItem('user', JSON.stringify({
            token: response.data.token,
            role: response.data.role
        }));
    return response.data;
};

const register = async (email, password, role) => {
    try {
        const response = await axios.post(`${BASE_URL}/auth/register`, { email, password, role });
        return response.data;
    } catch (error) {
        if (error.response && error.response.data) {
            throw error.response.data;
        }
        throw new Error('Registration failed');
    }
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