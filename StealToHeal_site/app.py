from flask import Flask, render_template, request, redirect, url_for, session
from models.user import User
from utils.auth import authenticate_user
from database.db_manager import get_db

app = Flask(__name__)
app.secret_key = 'supersecretkey'


# --- Home Route ---
@app.route('/')
def home():
    if 'user' in session:
        return render_template('index.html', username=session['user'])
    return redirect('/login')


# --- Login Route ---
@app.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        username = request.form['username']
        password = request.form['password']

        if authenticate_user(username, password):
            session['user'] = username
            return redirect('/')
        else:
            return "Invalid credentials. Try again!"

    return render_template('login.html')


# --- Signup Route ---
@app.route('/signup', methods=['POST'])
def signup():
    username = request.form['username']
    password = request.form['password']

    user = User(username=username, password=password)
    user.save()

    session['user'] = username
    return redirect('/')


# --- Logout Route ---
@app.route('/logout')
def logout():
    session.pop('user', None)
    return redirect('/login')


# --- Download Route ---
@app.route('/download')
def download_game():
    if 'user' in session:
        return redirect('/static/game.zip')
    return redirect('/login')


if __name__ == "__main__":
    app.run(debug=True)
