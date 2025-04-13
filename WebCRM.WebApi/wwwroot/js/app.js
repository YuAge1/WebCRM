class App {
    constructor() {
        this.currentUser = null;
        this.initializeEventListeners();
        this.checkAuth();
    }

    initializeEventListeners() {
        // Auth tabs
        document.querySelectorAll('.auth-tab').forEach(tab => {
            tab.addEventListener('click', () => this.switchAuthForm(tab.dataset.form));
        });

        // Auth forms
        document.getElementById('loginForm').addEventListener('submit', (e) => this.handleLogin(e));
        document.getElementById('registerForm').addEventListener('submit', (e) => this.handleRegister(e));

        // Logout
        document.getElementById('logoutBtn').addEventListener('click', () => this.handleLogout());

        // Navigation
        document.querySelectorAll('.nav-menu a').forEach(link => {
            link.addEventListener('click', (e) => this.handleNavigation(e));
        });
    }

    async checkAuth() {
        const token = localStorage.getItem('token');
        if (token) {
            try {
                // Verify token and get user info
                this.showApp();
                await this.loadDashboard();
            } catch (error) {
                this.showAuth();
            }
        } else {
            this.showAuth();
        }
    }

    switchAuthForm(form) {
        document.querySelectorAll('.auth-tab').forEach(tab => {
            tab.classList.toggle('active', tab.dataset.form === form);
        });
        document.getElementById('loginForm').classList.toggle('hidden', form !== 'login');
        document.getElementById('registerForm').classList.toggle('hidden', form !== 'register');
    }

    async handleLogin(e) {
        e.preventDefault();
        const form = e.target;
        try {
            const response = await api.login(
                form.email.value,
                form.password.value
            );
            this.currentUser = response;
            this.showApp();
            await this.loadDashboard();
        } catch (error) {
            alert(error.message);
        }
    }

    async handleRegister(e) {
        e.preventDefault();
        const form = e.target;
        try {
            const response = await api.register({
                username: form.username.value,
                firstName: form.firstName.value,
                lastName: form.lastName.value,
                email: form.email.value,
                phone: form.phone.value,
                password: form.password.value
            });
            this.currentUser = response;
            this.showApp();
            await this.loadDashboard();
        } catch (error) {
            alert(error.message);
        }
    }

    handleLogout() {
        api.clearToken();
        this.currentUser = null;
        this.showAuth();
    }

    showAuth() {
        document.getElementById('authForms').classList.remove('hidden');
        document.getElementById('dashboard').classList.add('hidden');
        document.querySelector('.sidebar').classList.add('hidden');
    }

    showApp() {
        document.getElementById('authForms').classList.add('hidden');
        document.getElementById('dashboard').classList.remove('hidden');
        document.querySelector('.sidebar').classList.remove('hidden');
        if (this.currentUser) {
            document.getElementById('userFullName').textContent = 
                `${this.currentUser.firstName} ${this.currentUser.lastName}`;
        }
    }

    async loadDashboard() {
        try {
            // Load statistics
            const analytics = await api.getAnalytics(
                new Date(new Date().setMonth(new Date().getMonth() - 6)),
                new Date()
            );

            // Update statistics
            document.getElementById('totalLeads').textContent = analytics.totalLeads;
            document.getElementById('activeDeals').textContent = analytics.totalDeals;
            document.getElementById('tasksToday').textContent = analytics.activeTasks;
            document.getElementById('monthlyRevenue').textContent = 
                `$${analytics.totalRevenue.toLocaleString()}`;

            // Update charts
            charts.updateDealsChart({
                labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
                values: [10000, 15000, 8000, 20000, 12000, 25000] // Example data
            });

            charts.updateLeadsChart({
                new: analytics.totalLeads * 0.4,
                contacted: analytics.totalLeads * 0.3,
                qualified: analytics.totalLeads * 0.2,
                lost: analytics.totalLeads * 0.1
            });

        } catch (error) {
            console.error('Error loading dashboard:', error);
        }
    }

    handleNavigation(e) {
        e.preventDefault();
        const page = e.target.dataset.page;
        
        // Update active state
        document.querySelectorAll('.nav-menu a').forEach(link => {
            link.classList.toggle('active', link.dataset.page === page);
        });

        // Hide all pages
        document.querySelectorAll('.page').forEach(p => p.classList.add('hidden'));

        // Show selected page
        document.getElementById(page).classList.remove('hidden');

        // Load page data if needed
        if (page === 'dashboard') {
            this.loadDashboard();
        }
    }
}

// Initialize app
const app = new App(); 