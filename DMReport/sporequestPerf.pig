Register '/home/hadoop/DM/myUDF.py' using jython as myfuncs;

Path0 = load 'hdfs://v2namenode:8020/spo/request/requestcommon' using PigStorage('\t') as (timestampUtc:chararray, userLogin:chararray, documentPath:chararray, referrerUrl:chararray, requestType:chararray, duration:double, serverUrl:chararray, browser:chararray, httpStatus:chararray);
Path = filter Path0 by (documentPath matches '.*/15/.*') AND (documentPath matches '.*.aspx') AND (NOT documentPath matches '.*authenticate.aspx') AND (NOT documentPath matches '.*signout.aspx');

FPath = foreach Path generate myfuncs.pagename(documentPath) as doc,myfuncs.day(timestampUtc) as day,duration;


GroupPath = group FPath by (doc,day);


PathCount = foreach GroupPath generate group,COUNT(FPath) as count, MIN(FPath.duration) as min, MAX(FPath.duration) as max, AVG(FPath.duration) as avgDuration, myfuncs.percentile(FPath.duration,50) as per50,myfuncs.percentile(FPath.duration,66) as per66, myfuncs.percentile(FPath.duration,95) as per95;
/*
describe GroupPath;
*/
describe PathCount;
dump PathCount;
store PathCount into 'hdfs://v2namenode:8020/spo/tmp/sporequestPerf' using PigStorage(',');
