using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Dtos;
using StudentManagement.Entities;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        public StudentController(IStudentService studentService,IMapper mapper)
        {
            this._studentService = studentService;
            this._mapper = mapper;
        }
        /// <summary>
        /// برای استفاده نیاز به احراز هویت کاربر می باشد
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            var model = _studentService.GetAll();

            return Ok(model);
        }

        /// <summary>
        /// برای استفاده نیاز به احراز هویت کاربر می باشد
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet("{studentId:int}")]
        [Authorize]
        public IActionResult GetById(int studentId) { 

            var model = _studentService.GetById(studentId);

            return Ok(model);
        }

        /// <summary>
        /// برای استفاده نیاز به احراز هویت ادمین می باشد
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles ="admin")]
        public IActionResult Create([FromBody] StudentDto model) 
        {
            
            if (!ModelState.IsValid) 
                return BadRequest();
            if (model.Name==model.Family)
            {
                ModelState.AddModelError("NameAndFamily", "نام و فامیلی نباید یکی باشد");
                return BadRequest(ModelState);
            }

            if (_studentService.CheckFamilyAndName(model.Family,model.Name)) {

                ModelState.AddModelError("NameAndFamily", "این نام و فامیلی قبلا ثبت شده");
                
                return BadRequest(ModelState);
            }
            
            var student=_mapper.Map<Student>(model);

            if (_studentService.Create(student))
            {
                var studentDto = _mapper.Map<StudentDto>(student);

                return StatusCode(201, studentDto);
            }

            ModelState.AddModelError("", "خطای سرور مجدد تلاش کنید");
            return StatusCode(500, ModelState);
        }

        /// <summary>
        /// برای استفاده نیاز به احراز هویت ادمین می باشد
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("{studentId:int}")]
        [Authorize(Roles = "admin")]
        public IActionResult Update(int studentId, [FromBody]StudentDto model) {
            if (!ModelState.IsValid) return BadRequest();

            if (studentId != model.Id) return NotFound();

            if (model.Name==model.Family) {
                ModelState.AddModelError("NameAndFamily","نام و فامیلی نباید یکی باشد");
                return BadRequest(ModelState);
            }

            if (_studentService.CheckFamilyAndName(model.Family,model.Name))
            {
                ModelState.AddModelError("NameAndFamily", "این نام و فامیلی قبلا ثبت شده");

                return BadRequest(ModelState);
            }

            var student=_studentService.Update(_mapper.Map<Student>(model));

            if (student) return StatusCode(200, model);


            else return StatusCode(500);
        }

        /// <summary>
        /// برای استفاده نیاز به احراز هویت ادمین می باشد
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpDelete("{studentId:int}")]
        [Authorize(Roles = "admin")]
        public IActionResult Remove(int studentId) {

            var student=_studentService.GetById(studentId);

            _studentService.Delete(student);

            return StatusCode(204);
        }

    }
}

