
import sys
data = sys.stdin.readlines()

for line in data:
    parts = line.split(",")
    key = parts[0][1:];
    column = parts[1][0:-1];
    value1 = parts[2][0:-1];
    value2 = parts[3][0:-1];

    print "put 'sposession','" + key + "','data:" + column + "_count','" + value1 + "'"
    print "put 'sposession','" + key + "','data:" + column + "_length','" + value2 + "'"
