from database.db_manager import get_db
from flask_bcrypt import Bcrypt

bcrypt = Bcrypt()

class User:
    def __init__(self, username, password):
        self.username = username
        self.password = bcrypt.generate_password_hash(password).decode('utf-8')
    
    def save(self):
        db = get_db()
        cursor = db.cursor()
        try:
            cursor.execute("INSERT INTO users (username, password) VALUES (?, ?)", (self.username, self.password))
            db.commit()
        except Exception as e:
            print(f"Error: {e}")
            db.rollback()


    @staticmethod
    def get_by_username(username):
        db = get_db()
        cursor = db.cursor()
        cursor.execute("SELECT * FROM users WHERE username = ?", (username,))
        user = cursor.fetchone()
        if user:
            return User(user[1], user[2])  # username, password
        return None
