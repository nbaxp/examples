# Gemba

## Shift Editor

```mermaid
flowchart LR
Day-->Date
Day-->Shift-->s1(Start_Time)
Shift-->e1(End_Time)
Shift-->Planned_Downtime_List-->Planned_Downtime_Item-->Downtime_Reason
Planned_Downtime_Item-->Duration_Mins
Planned_Downtime_Item-->Frequency
Planned_Downtime_Item-->c1(Comment)
Shift-->Production_Runs_List-->Part_item-->Part
Part_item-->Op_Code
Part_item-->s2(Start_Time)
Part_item-->e2(End_Time)
Part_item-->Standard_Rate_UPM
Part_item-->Speed_UPM
Part_item-->Operator
Part_item-->Outputs_List-->Output_Item-->Code-->Total_Units_Produced
Code-->Rejected_and_returned_for_rework
Code-->Scrap_Items
Output_Item -->b3(Behaviour)
Output_Item-->Quantity
Output_Item-->c2(Comment)
Part_item-->Event_List-->Event_Item-->Event_Reason
Event_Reason-->Belt_feed_issues
Event_Reason-->Broken_Skate_Assembler
Event_Reason-->Changeover_Overrun
Event_Reason-->Line_side_inventory_short
Event_Item-->b2(Behaviour)
Event_Item-->d2(Duration_Mins)
Event_Item-->f2(Frequency)
Event_Item-->c3(Comment)
Part_item-->Perofrmance_losse_List-->Perofrmance_losse_Item-->Perofrmance_losse_Reason
Perofrmance_losse_Item-->d3(Duration_Mins)
Perofrmance_losse_Item-->f3(Frequency)
Perofrmance_losse_Item-->c4(Comment)
```

## 班次编辑

```mermaid
flowchart LR
日记录-->日期
日记录-->班次记录-->s1(开始时间)
班次记录-->e1(结束时间)
班次记录-->计划停机时间列表-->计划停机记录-->停机原因
计划停机记录-->持续时间
计划停机记录-->出现次数
计划停机记录-->c1(备注)
班次记录-->生产运行列表-->生产运行记录-->零件
生产运行记录-->操作码
生产运行记录-->s2(开始时间)
生产运行记录-->e2(结束时间)
生产运行记录-->理论节拍
生产运行记录-->运行节拍
生产运行记录-->操作员
生产运行记录-->产出列表-->产出记录-->编码-->总产出
编码-->返工
编码-->废品
产出记录 -->b2(行为)
产出记录-->数量
产出记录-->c2(备注)
生产运行记录-->事件列表-->事件记录-->事件原因
事件原因-->Belt_feed_issues
事件原因-->Broken_Skate_Assembler
事件原因-->Changeover_Overrun
事件原因-->Line_side_inventory_short
事件记录-->b3(行为)
事件记录-->d2(持续时间)
事件记录-->f2(出现次数)
事件记录-->c3(备注)
生产运行记录-->性能损失列表-->性能损失记录-->性能损失原因
性能损失记录-->d3(持续时间)
性能损失记录-->f3(出现次数)
性能损失记录-->c4(备注)
```

## OEE数据格式分析

<table>
<tr>
<td>日期</td>
<td>班次</td>
<td>开始时间</td>
<td>结束时间</td>
<td>班次持续时间</td>
<td>类型</td>
<td>原因</td>
</tr>
<tr>
<td>-</td>
<td>-</td>
<td>-</td>
<td>-</td>
<td>-</td>
<td>计划停机</td>
<td>停机原因</td>
</tr>
<tr>
<td>-</td>
<td>-</td>
<td>-</td>
<td>-</td>
<td>-</td>
<td>生产运行</td>
</tr>
</table>
