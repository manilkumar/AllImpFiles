# Python program to convert text
# file to JSON

#import json 
#import pandas as pd


#read_file = pd.read_csv (r'TimeSheets.txt', header = None)
#read_file.columns = ['first_column','second_column',...]
#read_file.to_csv (r'TimeSheets1.txt.csv', index=None)

# the file to be converted to
# json format
filename = 'TimeSheets.txt'

list = []
# dictionary where the lines from
# text will be stored
dict1 = {} 

# creating list
with open(filename) as jfdsjlf: 
	for line in jfdsjlf:
	  list.append(line)

list = [item for item in list if ("-" not in item) and ("Time statement list" not in item) and ("Page" not in item) and ("Individual results" not in item)]

for item in list:
	split = item.split(":")
	print(item)

#with open(filename) as fh:
#	for line in fh:
#		if ":" in line:
#			try:
#			  #command, description = line.split(":")
#			  data = line.split(":")
#			  print(data)
#			  dict1[command] = description.strip() 
#			except:
#			  print("An exception occurred")
	
		

# creating json file 
# the JSON file is named as test1 
#out_file = open("test1.json", "w") 
#json.dump(dict1, out_file, indent = 4, sort_keys = False) 
#out_file.close() 

