from models.user import User
from flask_bcrypt import Bcrypt

bcrypt = Bcrypt()

def authenticate_user(username, password):
    user = User.get_by_username(username)
    if user and bcrypt.check_password_hash(user.password, password):
        return True
    return False
