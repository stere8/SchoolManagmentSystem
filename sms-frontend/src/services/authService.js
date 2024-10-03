import axios from 'axios';
import {BASE_URL} from '../settings';

const login = async (email, password) => {
    try {
        const response = await axios.post(`${BASE_URL}/auth/login`, {email, password});
        localStorage.setItem('user', JSON.stringify({
            token: response.data.token,
            role: response.data.role
        }));
        console.log('Login successful in auth service'); // Log success message

        return response.data;
    } catch (error) {
        console.error('Login failed. Auth Service:', error);
        throw error; // rethrow the error if you want it to propagate
    }
};


const register = async (email, password, role) => {
    try {
        const response = await axios.post(`${BASE_URL}/auth/register`, {email, password, role});
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

const getCurrentUserInfo = async () => {
    try {
        const user = JSON.parse(localStorage.getItem('user'));
        if (!user || !user.token) {
            throw new Error('User is not logged in');
        }

        const response = await axios.get(`${BASE_URL}/users/current`, {
            headers: {
                Authorization: `Bearer ${user.token}`
            }
        });

        // Update local storage with additional user info
        localStorage.setItem('user', JSON.stringify({
            ...user,
            name: response.data.name // Assuming the response includes the user's name
        }));

        return response.data;
    } catch (error) {
        console.error('Failed to fetch user info:', error);
        throw error;
    }
};

export default {
    login,
    register,
    logout,
    getCurrentUser,
    getCurrentUserInfo,
};