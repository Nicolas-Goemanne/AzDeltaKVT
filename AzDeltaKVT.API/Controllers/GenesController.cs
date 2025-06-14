﻿using AzDektaKVT.Model;
using AzDeltaKVT.Core;
using AzDeltaKVT.Dto.Requests;
using AzDeltaKVT.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace AzDeltaKVT.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class GenesController : ControllerBase
	{
		private readonly GeneService _geneService;
		private readonly TranscriptService _transcriptService;

		public GenesController(GeneService geneService, TranscriptService transcriptService)
		{
			_geneService = geneService;
			_transcriptService = transcriptService;
		}

		// GET /genes
		[HttpGet]
		public async Task<IActionResult> Find()
		{
			var result = await _geneService.Find();
			return Ok(result);
		}

		// POST /genes/get
		[HttpPost("get")]
		public async Task<IActionResult> Get([FromBody] GeneRequest request)
		{
			var genes = await _geneService.Get(request);
			return Ok(genes);
		}

		// POST /genes/create
		[HttpPost("create")]
		public async Task<IActionResult> Create([FromBody] GeneRequest request)
		{
			var transcript = await _transcriptService.Get(request.Nm_Number);
            if (transcript != null)
            {
                return BadRequest(new { message = "Transcript already exists for this NM number." });
            }

            var existingGene = await _geneService.GetByName(request.Name);
            if (existingGene != null)
            {
                return BadRequest(new { message = "Gene with this name already exists." });
            }

            var createdGene = await _geneService.Create(request);
            return Ok(createdGene);
        }

		// PUT /genes/update
		[HttpPut("update")]
		public async Task<IActionResult> Update([FromBody] GeneRequest request)
		{
			var updated = await _geneService.Update(request);
			return Ok(updated);
		}

		// DELETE /genes/delete
		[HttpDelete("delete")]
		public async Task<IActionResult> Delete([FromBody] string name)
		{
			var deleted = await _geneService.Delete(name);
			return Ok(deleted);
		}
	}
}