import { axiosInstance } from '../settings';

const login = async (email, password) => {
    try {
        const response = await axiosInstance.post('/auth/login', { email, password });
        localStorage.setItem('user', JSON.stringify({
            token: response.data.token,
            role: response.data.role
        }));
        console.log('Login successful in auth service'); // Log success message
        return response.data;
    } catch (error) {
        console.error('Login failed. Auth Service:', error);
        throw error.response ? error.response.data : error.message;
    }
};

// Rest of the functions remain the same...
const register = async (email, password, role) => {
    try {
        const response = await axiosInstance.post('/auth/register', { email, password, role });
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

        const response = await axiosInstance.get('/users/current', {
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
