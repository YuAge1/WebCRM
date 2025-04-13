class Api {
    constructor(baseUrl = '/api') {
        this.baseUrl = baseUrl;
        this.token = localStorage.getItem('token');
    }

    setToken(token) {
        this.token = token;
        localStorage.setItem('token', token);
    }

    clearToken() {
        this.token = null;
        localStorage.removeItem('token');
    }

    async request(endpoint, options = {}) {
        const url = `${this.baseUrl}${endpoint}`;
        const headers = {
            'Content-Type': 'application/json',
            ...(this.token ? { 'Authorization': `Bearer ${this.token}` } : {}),
            ...options.headers
        };

        try {
            const response = await fetch(url, {
                ...options,
                headers
            });

            if (!response.ok) {
                const error = await response.json();
                throw new Error(error.message || 'Something went wrong');
            }

            return await response.json();
        } catch (error) {
            console.error('API Error:', error);
            throw error;
        }
    }

    // Auth
    async login(email, password) {
        const response = await this.request('/accounts/login', {
            method: 'POST',
            body: JSON.stringify({ email, password })
        });
        this.setToken(response.token);
        return response;
    }

    async register(userData) {
        const response = await this.request('/accounts/register', {
            method: 'POST',
            body: JSON.stringify(userData)
        });
        this.setToken(response.token);
        return response;
    }

    // Leads
    async getLeads() {
        return await this.request('/leads');
    }

    async createLead(leadData) {
        return await this.request('/leads', {
            method: 'POST',
            body: JSON.stringify(leadData)
        });
    }

    // Deals
    async getDeals() {
        return await this.request('/deals');
    }

    async createDeal(dealData) {
        return await this.request('/deals', {
            method: 'POST',
            body: JSON.stringify(dealData)
        });
    }

    // Tasks
    async getTasks() {
        return await this.request('/tasks');
    }

    async createTask(taskData) {
        return await this.request('/tasks', {
            method: 'POST',
            body: JSON.stringify(taskData)
        });
    }

    // Analytics
    async getAnalytics(startDate, endDate) {
        return await this.request(`/analytics?startDate=${startDate}&endDate=${endDate}`);
    }
}

const api = new Api(); 