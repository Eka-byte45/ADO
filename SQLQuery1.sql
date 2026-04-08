IF NOT EXISTS (
SELECT stud_id FROM Students 
WHERE	last_name=N'sdlkjfslkdjf' 
AND		first_name=N'sldfjslkdjf' 
AND		birth_date=N'1977-10-24' 
AND		[group]=N'1' )
INSERT	Students (last_name,first_name,birth_date,[group]) VALUES (True,True,True,False)