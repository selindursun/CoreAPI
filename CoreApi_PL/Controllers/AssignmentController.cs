using CoreApi_BLL.Implementations;
using CoreApi_BLL.Interfaces;
using CoreApi_EL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApi_PL.Controllers
{
    //İşlemlerin sonucu nasıl geri dönmelidir ?
    //OK 200 --> Bu get işlemi başarılı anlamında
    //NoContent 204 --> Güncelleme işlemi
    //NoContent 204 --> Silme işlemi
    //Created 201 --> ekleme işlemi

    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public AssignmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult GetAllAssignments()
        {
            try
            {
                var list = _unitOfWork.AssignmentRepository.GetAll().ToList();
                return Ok(list);
            }
            catch (Exception)
            {
                // UI DAKİ catch lerin içinde throw olmalı !!!
                return StatusCode(500);

            }
        }
        [HttpGet("{id}")]
        //[HttpGet("[action]/{id}")]
        public IActionResult GetAssignmentById(int id)
        {
            try
            {
                var assignment = _unitOfWork.AssignmentRepository.GetFirstOrDefault(x => x.Id == id);
                if (assignment !=null)
                {
                    return Ok(assignment);
                }
                return Problem();
            }
            catch (Exception)
            {
                return Problem();
            }
        }
        [HttpPost]
        public IActionResult AddAssignment(Assignment model)
        {
            try
            {
                model.CreatedDate = DateTime.Now;
                bool result=_unitOfWork.AssignmentRepository.Add(model);
                if (result)
                {
                    return Created("", model);
                }
                return Problem();
            }
            catch (Exception)
            {

                return Problem();
            }
        }
        //api/Products/DeleteProduct?id 
        // public IActionResult DeleteProduct([FromQuery] int id) *FromQuery ? işareti ile göndermek

        //api/Assignment/1
        //[HttpDelete("{id}")]
        //api/Assignment/DeleteAssignment/1
        [HttpGet("[action]/{id}")]
        public IActionResult DeleteAssignment(int id)
        {
            try
            {
                if (id>0)
                {
                    var data = _unitOfWork.AssignmentRepository.GetFirstOrDefault(x => x.Id == id);
                    if (data !=null)
                    {
                        bool result = _unitOfWork.AssignmentRepository.Delete(data);
                        if (result)
                        {
                            return NoContent();
                        }
                        return Problem();
                    }
                    return Problem();
                }
                return Problem();
            }
            catch (Exception)
            {

                return Problem();
            }
        }
        //api/Assignment/id
        [HttpPut("{id}")]
        public IActionResult UpdateAssignment(int id,Assignment model)
        {
            try
            {
                if (id>0)
                {
                    var currentAssignment = _unitOfWork.AssignmentRepository.GetFirstOrDefault(x => x.Id == id);
                    if (currentAssignment != null)
                    {
                        currentAssignment.Description = string.IsNullOrEmpty(model.Description) ||model.Description=="XX" ? currentAssignment.Description : model.Description;
                        currentAssignment.IsCompleted = model.IsCompleted;
                        bool result = _unitOfWork.AssignmentRepository.Update(currentAssignment);
                        if (result)
                        {
                            return NoContent();
                        }
                        throw new Exception("Görev bulundu ama beklenmedik bir hata nedeniyle güncelleştirme başarısızdır!");
                    }
                    throw new Exception("Görev bulunamadı gönderdiğiniz id bilgisini kontrol ediniz");
                }
                throw new Exception("id sıfırdan büyük olmalıdır.");
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }
    
    }
}
