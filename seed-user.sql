
-- Add a default admin user for login
INSERT INTO AspNetUsers (Id, UserName, UserPassword, Email, EmailConfirmed)
VALUES ('1', 'admin', 'admin123', 'admin@example.com', 1);

-- Add admin role
INSERT INTO AspNetRoles (Id, Name)
VALUES ('1', 'Administrator');

-- Assign admin role to user
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES ('1', '1');
