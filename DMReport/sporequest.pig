Register '/home/hadoop/DM/myUDF.py' using jython as myfuncs;
Register '/home/hadoop/DM/myUDF_ly.py' using jython as myfuncs_ly;
Path = load 'hdfs://v2namenode:8020/spo/request/requestcommon' using PigStorage('\t') as (timestampUtc:chararray, userLogin:chararray, documentPath:chararray, referrerUrl:chararray, requestType:chararray, duration:double);
Path0 = filter Path by (documentPath matches '.*/15/.*') AND (documentPath matches '.*.aspx') AND (NOT documentPath matches '.*authenticate.aspx') AND (NOT documentPath matches '.*signout.aspx');
Path11 = foreach Path0 generate myfuncs.pagename(documentPath) as doc, timestampUtc;
Path1 = filter Path11 by (NOT doc matches 'picker') AND (NOT doc matches 'accessdenied');
GroupPath = group Path1 by doc;
PathCount = foreach GroupPath generate group, COUNT(Path1) as count;
STORE PathCount INTO 'hbase://sporequest' USING org.apache.pig.backend.hadoop.hbase.HBaseStorage('data:count');
FPath2 = foreach Path1 generate doc, myfuncs.day(timestampUtc) as day;
GroupPath = group FPath2 by (doc, day);
PathCount = foreach GroupPath generate group, COUNT(FPath2) as count;
store PathCount into 'hdfs://v2namenode:8020/spo/tmp/sporequest' using PigStorage(',');
FPathM = foreach Path1 generate doc,myfuncs_ly.month(timestampUtc) as month;
GroupPathM = group FPathM by (doc, month);
PathCountM = foreach GroupPathM generate group, COUNT(FPathM) as count;
store PathCountM into 'hdfs://v2namenode:8020/spo/tmp/sporequestMonthCount' using PigStorage(',');

