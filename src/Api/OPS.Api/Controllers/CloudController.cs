using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPS.Api.Common;
using OPS.Api.Common.ErrorResponses;
using OPS.Application.Dtos;
using OPS.Application.Features.CloudFiles.Commands;
using OPS.Application.Features.CloudFiles.Queries;
using OPS.Domain.Contracts.Core.GoogleCloud;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace OPS.Api.Controllers;

[Route("CloudFile")]
[ProducesResponseType<ValidationErrorResponse>(Status400BadRequest)]
[ProducesResponseType<ExceptionResponse>(Status500InternalServerError)]
public class CloudController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>Uploads a file to Google Cloud.</summary>
    /// <param name="file">File to upload (Max file size: 100 KB).</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Uploaded file information.</returns>
    [AllowAnonymous]
    [HttpPost("Upload")]
    [Consumes("multipart/form-data")]
    [EndpointDescription("Uploads a file to Google Cloud.")]
    [ProducesResponseType<CloudFileResponse>(Status200OK)]
    [ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
    public async Task<IActionResult> UploadAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var command = new UploadFileCommand(file);
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Gets file details from Google Cloud.</summary>
    /// <param name="cloudFileId">Cloud File ID.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>File details.</returns>
    [AllowAnonymous]
    [HttpGet("Details/{cloudFileId:guid}")]
    [ProducesResponseType<CloudFileResponse>(Status200OK)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> GetFileDetailsAsync(Guid cloudFileId, CancellationToken cancellationToken)
    {
        var query = new GetFileDetailsQuery(cloudFileId);
        var response = await _mediator.Send(query, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Edits a file from Google Cloud.</summary>
    /// <param name="cloudFileId">Cloud File ID.</param>
    /// <param name="file">File to edit.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Edited file information.</returns>
    [Authorize]
    [HttpPut("Edit/{cloudFileId}")]
    [Consumes("multipart/form-data")]
    [EndpointDescription("Edits a file in Google Cloud.")]
    [ProducesResponseType<CloudFileResponse>(Status200OK)]
    [ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> EditFileAsync(Guid cloudFileId, IFormFile file,
        CancellationToken cancellationToken)
    {
        var command = new EditFileCommand(cloudFileId, file);
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }

    /// <summary>Downloads a file from Google Cloud.</summary>
    /// <param name="fileId">File ID.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>File download information.</returns>
    [AllowAnonymous]
    [HttpGet("Download/{fileId}")]
    [ProducesResponseType<GoogleFileDownload>(Status200OK)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> DownloadFileAsync(string fileId, CancellationToken cancellationToken)
    {
        var command = new FileDownloadCommand(fileId);
        var response = await _mediator.Send(command, cancellationToken);

        if (response.IsError)
        {
            return ToResult(response);
        }

        var file = response.Value;
        return File(file.Bytes, file.ContentType, file.FileName);
    }

    /// <summary>Deletes a file from Google Cloud.</summary>
    /// <param name="cloudFileId">Cloud File ID.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Deletion result.</returns>
    [Authorize]
    [HttpDelete("Delete/{cloudFileId}")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType<UnauthorizedResponse>(Status401Unauthorized)]
    [ProducesResponseType<NotFoundResponse>(Status404NotFound)]
    public async Task<IActionResult> DeleteFileAsync(Guid cloudFileId, CancellationToken cancellationToken)
    {
        var command = new DeleteFileCommand(cloudFileId);
        var response = await _mediator.Send(command, cancellationToken);
        return ToResult(response);
    }
}