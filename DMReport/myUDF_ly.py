
from datetime import datetime

@outputSchema("day:chararray")
def month(date):
    date1=date.split(" ")[0];
    return date1[:-3];
#    month1 = date.split(" ")[0];
#    month2 = datetime.strptime(month1,'%Y-%m-%d');
#    return month2;
