#/usr/bin/jython

from datetime import datetime

@outputSchema("day:chararray")
def day(date):
  return date.split(" ")[0];

@outputSchema("yes:boolean")
def greaterthanday(day1, day2):
  date1 = datetime.strptime(day(day1), '%Y-%m-%d');
  date2 = datetime.strptime(day2, '%Y-%m-%d');
  return date1 > date2;

  
@outputSchema("result:double")
def percentile(values, percent):
	l = []
	for item in values:
		l.insert(0, item);
	l.sort();
	return l[len(l) * percent / 100];


@outputSchema("page:chararray")
def pagename(documentpath):
	s = documentpath.split("/");
	s = s[len(s)-1];
	s = s.split(".");
	return s[0];


@outputSchema("yes:chararray")
def avgwithoutzero(lengths):
	l = [];
	for item in lengths:
		for length in item:
			if (length > 0):
				l.insert(0, length);
	
	if (len(l) == 0):
		return 0;
	else:
		return sum(l)/len(l);


@outputSchema("yes:chararray")
def sessionCount(times):
	c = []
	for item in times:
		string2 = str(item)[3:-3];
		string2 = string2.split(".")[0]
		c.insert(0, datetime.strptime(string2, '%Y-%m-%d %H:%M:%S'));
	c.sort();

	last = datetime.strptime("1999-12-31", '%Y-%m-%d');
	sessionCount = 0;
	for item in c:
		current  = item;
		delta =	(current - last);
		if (delta.days > 0 or delta.seconds > 1800):
			sessionCount = sessionCount + 1;
		last = current;
	return sessionCount;
		

@outputSchema("yes:chararray")
def sessionLength(times):
	c = []
	for item in times:
		string2 = str(item)[3:-3];
		string2 = string2.split(".")[0]
		c.insert(0, datetime.strptime(string2, '%Y-%m-%d %H:%M:%S'));
	c.sort();
	
	if (len(c) == 0):
		return "empty list";
	
	sessionlength = []
	sessionstart = None;
	last = None;
	for item in c:
		if (sessionstart == None):
			sessionstart = item;
			last = item;

		current  = item;
		delta =	(current - last);
		if (delta.days > 0 or delta.seconds > 1800):
			length = (last - sessionstart).seconds + (last-sessionstart).days*24*3600;
			if (length > 120): # any session > 2 minutes makes sense to me
				sessionlength.insert(0, length);
			sessionstart = current;

		last = current;

	if (len(sessionlength) == 0):
		length = (last - sessionstart).seconds + (last-sessionstart).days*24*3600;
		if (length > 120): # any session > 2 minutes makes sense to me
			sessionlength.insert(0, length);
	
	if (len(sessionlength) == 0):
		return 0;
	return sum(sessionlength)/len(sessionlength);


@outputSchema("yes:chararray")
def mininumDate(days):
	string2 = "9999-12-13"
	minDate = datetime.strptime('9999-12-31', '%Y-%m-%d');
	for item in days:
		string2 = str(item)[3:-3]
		date = datetime.strptime(string2, '%Y-%m-%d');
		if (date < minDate):
			minDate = date;
	return str(minDate);


@outputSchema("yes:chararray")
def week(date):
	d = datetime.strptime(date, '%Y-%m-%d');
	i = d.isocalendar();
 	s = str(i[0]) + "-" +str(i[1]);
	return s;


@outputSchema("yes:boolean")
def isSameMonth(date1, date2):
	d1 = datetime.strptime(date1, '%Y-%m-%d');
	d2 = datetime.strptime(date2, '%Y-%m-%d');
	return d1.year == d2.year and d1.month == d2.month;


@outputSchema("minutes:int")
def o365sessionlength(times):
    c = []
    for item in times:
        string2 = str(item)[3:-3];
        string2 = string2.split(".")[0]
        c.insert(0, datetime.strptime(string2, '%m/%d/%Y %H:%M:%S %p'));
    c.sort();
	
    if (len(c) == 0):
        return "empty list";
	
    interval = c[len(c)-1]-c[0];
    length = interval.seconds + interval.days*24*3600;
    return length/60;


