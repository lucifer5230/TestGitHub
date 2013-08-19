import sys

#print "disable 'sporequest'"
#print "alter 'sporequest',{NAME => 'Perf'}"
#print "enable 'sporequest'"
data = sys.stdin.readlines()
for line in data:
    parts = line.split(",")
    key = parts[0][1:];
    column = parts[1][0:-1];
    valueMin = parts[3][0:];
    valueMax = parts[4][0:];
    valueAVG = parts[5][0:];
    valueP50 = parts[6][1:-1];
    valueP66 = parts[7][1:-1];
    valueP95 = parts[8][1:-2];

    print "put 'sporequest','" + key + "','Perf:" + column + "_Min','" + valueMin + "'"
    print "put 'sporequest','" + key + "','Perf:" + column + "_Max','" + valueMax + "'"

    print "put 'sporequest','" + key + "','Perf:" + column + "_AVG','" + valueAVG + "'"
    print "put 'sporequest','" + key + "','Perf:" + column + "_P50','" + valueP50 + "'"
    print "put 'sporequest','" + key + "','Perf:" + column + "_P66','" + valueP66 + "'"
    print "put 'sporequest','" + key + "','Perf:" + column + "_P95','" + valueP95 + "'"
