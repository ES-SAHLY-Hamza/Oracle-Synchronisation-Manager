namespace projet_oracle_web.Services
{
	public class Employee
	{
		public int EMPNO { get; set; }
		public string ENAME { get; set; }
		public string JOB { get; set; }
		public int? MGR { get; set; }
		public DateTime HIREDATE { get; set; }
		public decimal SAL { get; set; }
		public decimal? COMM { get; set; }
		public int DEPTNO { get; set; }
	}

}
