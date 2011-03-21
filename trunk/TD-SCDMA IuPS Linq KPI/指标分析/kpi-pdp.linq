<Query Kind="Statements">
  <Connection>
    <ID>337b631f-296e-4a3b-aa36-09f23a9a9a38</ID>
    <Server>.\SQLEXPRESS</Server>
    <Persist>true</Persist>
    <Database>RNC681_2008</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

TimeSpan timer=IuPS_PDPs.Max(e=>e.PacketTime).Value- IuPS_PDPs.Min(e=>e.PacketTime).Value;
var mTime=timer.TotalSeconds;
var tt=IuPS_PDPs.Min(e=>e.PacketTime).Value.ToString()+"~"+IuPS_PDPs.Max(e=>e.PacketTime).Value.ToString()+"~"+mTime.ToString();
tt.Dump();
var totalPDPRequest=IuPS_PDPs.Where (e=>e.DumpFor =="EndFlowByFlowDesigner").Sum(e=>e.PDP_Act_Request);
var a= from p in IuPS_PDPs.Where (e=>e.DumpFor =="EndFlowByFlowDesigner")
	group p by p.DumpFor into ps
//	orderby ps.Key
	select new {
	mKey=ps.Key ,
	mPDPRequest=ps.Sum (e=>e.PDP_Act_Request ),
	mPDPRequestRate=(ps.Sum(e=>e.PDP_Act_Request)+0.0)/totalPDPRequest,
	mPDPSuccRate=(ps.Sum (e=>e.PDP_Act_Accept )+0.0)/ps.Sum (e=>e.PDP_Act_Request ),
	mRABDelay=ps.Average(e=>e.RAB_AssignmentResponse_delayFirst),
	mPDPDelay=ps.Average (e=>e.PDP_Act_Accept_delayFirst)
	};
	a.OrderByDescending(e=>e.mPDPDelay ).Dump ();