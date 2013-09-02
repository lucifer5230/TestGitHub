from datetime import datetime
import sys
data = sys.stdin.readlines()

for line in data:
	parts = line.split("\t");
	user = parts[0];
	if not user: # Ignore empty user
		continue;
	requests = str(parts[1])[2:-3];
	requests = requests.split("),(");

	lastTime = datetime.strptime("1999-01-01", '%Y-%m-%d');
	l = [];
	
	for request in requests:
		#print request;
		parts = request.split(",");
		if len(parts) != 3:
			continue;
		path = parts[1];
		time = datetime.strptime(parts[2], '%Y-%m-%d %H:%M:%S.%f');
		delta = time - lastTime;
		if (delta.days > 0 or delta.seconds > 1800) and len(l) != 0:
			l.reverse();
			print ','.join(l);
			l = [];
		else:
			l.insert(0, path);
			lastTime = time;
