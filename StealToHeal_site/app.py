from flask import Flask, render_template, request, redirect, url_for, session, flash
from werkzeug.security import generate_password_hash, check_password_hash

app = Flask(__name__)
app.secret_key = 'your_secret_key_here'  # Secret key for session management

# Dummy database (In-memory for demonstration)
users_db = {
    'boyan': {
        'password': generate_password_hash("hacking_tues")
    }
}

# Home route (shows the home page)
@app.route('/')
def home():
    return render_template('index.html')

@app.route('/finance')
def finance():
    return render_template('finance.html')

@app.route('/health')
def health():
    return render_template('health.html')

@app.route('/time')
def time():
    return render_template('time.html')

# Login route (GET: show login form, POST: handle login)
@app.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        username = request.form['username']
        password = request.form['password']
        login_action = request.form['login-action']
        if login_action == 'register':
            return redirect('/register')
        
        # Check if user exists in the database
        if username in users_db:
            # Compare entered password with stored hashed password
            if check_password_hash(users_db[username]['password'], password):
                session['username'] = username
                flash('Login successful!', 'success')
                return redirect(url_for('home'))
            else:
                flash('Invalid password. Please try again.', 'danger')
        else:
            flash('Username not found. Please register first.', 'danger')
    
    return render_template('index.html', route='/login')

# Registration route (GET: show registration form, POST: handle registration)
@app.route('/register', methods=['GET', 'POST'])
def register():
    if request.method == 'POST':
        username = request.form['username']
        password = request.form['password']
        
        # Check if user already exists
        if username in users_db:
            flash('Username already taken. Please choose a different one.', 'danger')
            return redirect(url_for('register'))
        
        # Hash the password before storing it
        hashed_password = generate_password_hash(password)
        
        # Store the new user in the "database"
        users_db[username] = {'password': hashed_password}
        flash('Registration successful! Please log in now.', 'success')
        return redirect(url_for('login'))
    
    return render_template('register.html', route = '/register')

# Logout route (logs out the user)
@app.route('/logout')
def logout():
    session.pop('username', None)
    flash('You have been logged out.', 'info')
    return redirect(url_for('home'))

@app.route('/debug_users')
def debug_users():
    result = ''
    for key, value in users_db.items():
        result += key + '=>' + value['password'] + '\n\r'
    return result

# Run the Flask app
if __name__ == '__main__':
    app.run(debug=True)
