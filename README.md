# DataSync
一个基于Rocket MQ的不同数据库之间数据实时同步的平台

watcher为监视数据更新的windows service
<br>
配置文件：
<br>
1.\Config\producerconfig.json -- rocket mq的生产者配置，用来将从数据库查询出来的数据推送到mq以便同步进行同步消费
<br>
2.\Config\watcherConfig.json -- 数据监视配置，用来监视数据库变动
<br>


<br>
sync service为从rocket-mq取同步数据同步到数据库的windows service
<br>
配置文件：
<br>
1.\Config\consumerconfig.json -- rocket mq的消费者配置
<br>
2.\Config\syncDataConfig.json -- 同步数据配置，用来定义数据匹配和转换业务
<br>
<br>
请看SampleConfig文件中有示例配置
