using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using StudentManagement.Dtos;
using StudentManagement.Entities;
using StudentManagement.Resources;
using StudentManagement.Services;
using StudentManagement.Utility;

namespace StudentManagement.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<StudentManagement.Resources.Resource> _localizer;
        
        
        public StudentController(IStudentService studentService,IMapper mapper,IStringLocalizer<StudentManagement.Resources.Resource> localizer)
        {
            this._studentService = studentService;
            this._mapper = mapper;
            this._localizer = localizer;
            
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
                ModelState.AddModelError("NameAndFamily", _localizer["DuplicateNameAndFamilyMessage"].Value);
                return BadRequest(ModelState);
            }

            if (_studentService.CheckFamilyAndName(model.Family,model.Name)) {

                ModelState.AddModelError("NameAndFamily", _localizer["NameAndFamilyExistedMessage"].Value);
                
                return BadRequest(ModelState);
            }
            
            var student=_mapper.Map<Student>(model);

            if (_studentService.Create(student))
            {
                var studentDto = _mapper.Map<StudentDto>(student);

                return StatusCode(201, studentDto);
            }

            ModelState.AddModelError("", _localizer["ServerErrorMessage"].Value);
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
                ModelState.AddModelError("NameAndFamily", _localizer["NotSameNameAndFamilyMessage"].Value);
                return BadRequest(ModelState);
            }

            if (_studentService.CheckFamilyAndName(model.Family,model.Name))
            {
                ModelState.AddModelError("NameAndFamily", _localizer["NameAndFamilyExistedMessage"].Value);

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

