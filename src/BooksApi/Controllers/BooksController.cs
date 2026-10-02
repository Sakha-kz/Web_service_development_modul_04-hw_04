using AutoMapper;
using BooksApi.Models;
using BooksApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<BooksController> _logger;

    public BooksController(IBookRepository repository, IMapper mapper, ILogger<BooksController> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: /api/books
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Getting all books from catalog");
        var books = await _repository.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<BookDto>>(books);
        return Ok(ReturnResult<IEnumerable<BookDto>>.Success(dtos));
    }

    // GET: /api/books/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("Getting book with ID {BookId}", id);
        var book = await _repository.GetByIdAsync(id);
        if (book == null)
        {
            _logger.LogWarning("Book with ID {BookId} not found", id);
            return NotFound(ReturnResult<BookDto>.Failure($"Книга с ID {id} не найдена."));
        }

        var dto = _mapper.Map<BookDto>(book);
        return Ok(ReturnResult<BookDto>.Success(dto));
    }

    // POST: /api/books
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Author) || dto.Price < 0)
        {
            return BadRequest(ReturnResult<BookDto>.Failure("Название книги и автор обязательны, а цена не может быть отрицательной."));
        }

        var book = _mapper.Map<Book>(dto);
        await _repository.CreateAsync(book);

        var createdDto = _mapper.Map<BookDto>(book);
        _logger.LogInformation("Book '{BookTitle}' created with ID {BookId}", book.Title, book.Id);

        return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, ReturnResult<BookDto>.Success(createdDto));
    }

    // PUT: /api/books/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BookDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Author))
        {
            return BadRequest(ReturnResult<BookDto>.Failure("Некорректные данные для обновления книги."));
        }

        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            _logger.LogWarning("Book with ID {BookId} not found for update", id);
            return NotFound(ReturnResult<BookDto>.Failure($"Книга с ID {id} не найдена."));
        }

        _mapper.Map(dto, existing);
        existing.Id = id;

        await _repository.UpdateAsync(existing);
        var updatedDto = _mapper.Map<BookDto>(existing);

        _logger.LogInformation("Book with ID {BookId} successfully updated", id);
        return Ok(ReturnResult<BookDto>.Success(updatedDto));
    }

    // DELETE: /api/books/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            _logger.LogWarning("Book with ID {BookId} not found for deletion", id);
            return NotFound(ReturnResult<bool>.Failure($"Книга с ID {id} не найдена."));
        }

        await _repository.DeleteAsync(id);
        _logger.LogInformation("Book with ID {BookId} successfully deleted", id);

        return Ok(ReturnResult<bool>.Success(true));
    }

    // GET: /api/books/author/{author} (Поиск книг по автору)
    [HttpGet("author/{author}")]
    public async Task<IActionResult> GetByAuthor(string author)
    {
        _logger.LogInformation("Searching books by author: {Author}", author);
        var books = await _repository.GetByAuthorAsync(author);
        var dtos = _mapper.Map<IEnumerable<BookDto>>(books);

        return Ok(ReturnResult<IEnumerable<BookDto>>.Success(dtos));
    }
}
