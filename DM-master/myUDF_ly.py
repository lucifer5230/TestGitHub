
from datetime import datetime

@outputSchema("day:chararray")
def month(date):
    date1=date.split(" ")[0];
    return date1[:-3];


@outputSchema("page:chararray")
def pagename_ly(documentpath):
    s = documentpath.split("/");
    s = s[len(s)-1];
    s = s.split(".");
    return s[0];


#    month1 = date.split(" ")[0];
#    month2 = datetime.strptime(month1,'%Y-%m-%d');
#    return month2;
