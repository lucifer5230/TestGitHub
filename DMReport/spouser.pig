
--to get the spouser
Path0 = load 'hdfs://v2namenode:8020/spo/request/requestcommon' using PigStorage('\t') as (timestampUtc:chararray, userLogin:chararray, documentPath:chararray, referrerUrl:chararray, requestType:chararray, duration:double);
Path = filter Path0 by (userLogin matches '.*@.*') AND (NOT documentPath matches '.*getsitecollectionservice.asmx.*');
Path1 = foreach Path generate myfuncs.day(timestampUtc) as date, userLogin as user;
GroupPath = group Path1 by user;
Path2 = foreach GroupPath generate group as user, myfuncs.mininumDate(Path1.date) as miniDate;
Path3 = foreach Path2 generate user, myfuncs.week(miniDate);
STORE Path3 INTO 'hbase://spouser' USING org.apache.pig.backend.hadoop.hbase.HBaseStorage('profile:firstlogin');
