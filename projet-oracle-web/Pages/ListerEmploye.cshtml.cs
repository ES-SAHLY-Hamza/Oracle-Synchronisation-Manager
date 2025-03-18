using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using projet_oracle_web.Services;

namespace projet_oracle_web.Pages
{
	public class ListerEmployeModel : PageModel
	{
		private readonly EmployeeService _employeeService;

		public ListerEmployeModel(EmployeeService employeeService)
		{
			_employeeService = employeeService;
		}

		public List<Employee> Employees { get; set; }

		public async Task OnGetAsync()
		{
			Employees = await _employeeService.GetEmployeesAsync();
		}
	}
}
