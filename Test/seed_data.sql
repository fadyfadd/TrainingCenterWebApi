TRUNCATE TABLE application_users, administrators, students, courses_categories, courses, courses_students RESTART IDENTITY CASCADE;


ALTER SEQUENCE course_category_sequence RESTART WITH 1;
ALTER SEQUENCE course_sequence RESTART WITH 1;
ALTER SEQUENCE student_sequence RESTART WITH 1;
ALTER SEQUENCE administarator_sequence RESTART WITH 1;
 
INSERT INTO application_users ("UserName" , "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount") 
VALUES ('ziad.achkar',  'ZIAD.ACHKAR', 'ziad.achkar@yahoo.com', 'ZIAD.ACHKAR@YAHOO.COM', true, 'AQAAAAIAAYagAAAAEFcJZk6icd6e/yHe6EFqhxtFsC1mTCl2d9b3UV2lM6ESDPkvjOrWHNKj37Vb+1O/Mw==', '3EZLB3M7NNPVXHBNJNOXAO52KQBXOPCD', 'F6E5D4C3-B2A1-0F9E-8D7C-6B5A4M3L2K1J', '03-897333', true, false, NULL, true, 0);

INSERT INTO application_users ("UserName" , "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount") 
VALUES ('jean.tannous',  'JEAN.TANNOUS', 'jean.tannous@gmail.com', 'JEAN.TANNOUS@GMAIL.COM', true, 'AQAAAAIAAYagAAAAEFcJZk6icd6e/yHe6EFqhxtFsC1mTCl2d9b3UV2lM6ESDPkvjOrWHNKj37Vb+1O/Mw==', '3EZLB3M7NNPVXHBNJNOXAO52KQBXOPCD', 'F6E5D4C3-B2A1-0F9E-8D7C-6B5A4M3L2K1J', '03-397543', true, false, NULL, true, 0);


INSERT INTO students ("first_name" , "last_name" , "enrolled_at" , "user_id") values ('ziad' , 'achkar' , current_timestamp , 1);
INSERT INTO administrators ("first_name" , "last_name" , "enrolled_at" , "user_id") values ('jean' , 'tannous' , current_timestamp , 2);


Insert Into courses_categories ("name") values ('Software Engineering');

Insert Into courses ("title" , "course_category_id") values ('ASP.NET Core Web API' , 1);
Insert Into courses ("title" , "course_category_id") values ('Java Fundamentals' , 1);
Insert Into courses ("title" , "course_category_id") values ('Python for Data Science' , 1);
Insert Into courses ("title" , "course_category_id") values ('C# Introduction', 1);
Insert Into courses ("title" , "course_category_id") values ('JavaScript Basics', 1);
Insert Into courses ("title" , "course_category_id") values ('Database Design', 1);
Insert Into courses ("title" , "course_category_id") values ('Cloud Computing', 1);
Insert Into courses ("title" , "course_category_id") values ('DevOps Essentials', 1);
Insert Into courses ("title" , "course_category_id") values ('Data Structures Fundamentals', 1);
Insert Into courses ("title" , "course_category_id") values ('Algorithms Basics', 1);
Insert Into courses ("title" , "course_category_id") values ('React for Beginners', 1);


Insert Into courses_students ("student_id" , "course_id") values (1, 1);
Insert Into courses_students ("student_id" , "course_id") values (1, 2);
Insert Into courses_students ("student_id" , "course_id") values (1, 3);
Insert Into courses_students ("student_id" , "course_id") values (1, 4);
Insert Into courses_students ("student_id" , "course_id") values (1, 5);