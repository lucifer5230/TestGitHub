import sys
data = sys.stdin.readlines()

for line in data:
    parts = line.split(",")
    key = parts[0][1:];
    column = parts[1][0:-1];
    value1 = parts[2][0:-1];
#    print "delete 'sporequest','" + key + "','data:count_"+column + "'"
    print "put 'sporequest','" + key + "','data:count_" + column + "','" + value1 + "'"
