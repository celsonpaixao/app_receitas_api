using api_receita.DAL.Database;
using api_receita.DTO;
using api_receita.Models;
using app_receitas_api.DAL.Interfaces;
using app_receitas_api.Models;
using Microsoft.EntityFrameworkCore;

namespace app_receitas_api.DAL.Repositorys
{
    public class RecipeRepository : IRecipe
    {
        private readonly ReceitasDbContext dbContext;
        public RecipeRepository(ReceitasDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<DTOResponse> Update_Recipe(int id_receita, RecipeModel receitaAtualizada, IFormFile? newImage)
        {
            DTOResponse response = new DTOResponse();
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var receita = await dbContext.Tb_Receita.FindAsync(id_receita);
                if (receita == null)
                {
                    response.message = "Receita não encontrada";
                    return response;
                }

                // Atualiza os campos da receita
                receita.Title = receitaAtualizada.Title;
                receita.Description = receitaAtualizada.Description;
                receita.Instructions = receitaAtualizada.Instructions;

                // Se uma nova imagem for fornecida
                if (newImage != null && newImage.Length > 0)
                {
                    // Se a receita já tiver uma imagem, exclua-a
                    if (!string.IsNullOrEmpty(receita.ImageURL))
                    {
                        var imagePath = Path.Combine("wwwroot", receita.ImageURL);
                        if (File.Exists(imagePath))
                        {
                            File.Delete(imagePath);
                        }
                    }

                    // Salva a nova imagem
                    var uploadsFolder = Path.Combine("wwwroot", "uploads", "recipes");
                    var uniqueFileName = DateTime.Now.Ticks + Path.GetExtension(newImage.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Criar o diretório se não existir
                    Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await newImage.CopyToAsync(fileStream);
                    }

                    // Atualiza o caminho da imagem da receita
                    receita.ImageURL = Path.Combine("uploads", "recipes", uniqueFileName);
                }

                // Atualiza as categorias associadas
                var existingCategories = await dbContext.Tb_RecipeCategory
                    .Where(rc => rc.RecipeId == receita.Id)
                    .ToListAsync();
                dbContext.Tb_RecipeCategory.RemoveRange(existingCategories);

                foreach (var categoriaId in receitaAtualizada.Categorias)
                {
                    var receitaCategoria = new Recipe_CategoryModel
                    {
                        CategoryId = categoriaId,
                        RecipeId = receita.Id
                    };
                    dbContext.Tb_RecipeCategory.Add(receitaCategoria);
                }

                // Atualiza os ingredientes associados
                var existingIngredients = await dbContext.Tb_Ingredients_Recipe
                    .Where(ir => ir.RecipeId == receita.Id)
                    .ToListAsync();
                dbContext.Tb_Ingredients_Recipe.RemoveRange(existingIngredients);

                foreach (var ingredientName in receitaAtualizada.Ingredients)
                {
                    var existingIngredient = await dbContext.Tb_Ingredients.FirstOrDefaultAsync(i => i.Name == ingredientName);
                    if (existingIngredient == null)
                    {
                        existingIngredient = new IngredientsModel { Name = ingredientName };
                        dbContext.Tb_Ingredients.Add(existingIngredient);
                        await dbContext.SaveChangesAsync(); // Salva o novo ingrediente no banco de dados
                    }
                    var ingredientRecipe = new IngredientRecipeModel
                    {
                        IngredientId = existingIngredient.Id,
                        RecipeId = receita.Id
                    };
                    dbContext.Tb_Ingredients_Recipe.Add(ingredientRecipe);
                }

                // Atualiza os materiais associados
                var existingMaterials = await dbContext.Tb_Materials_Recipe
                    .Where(mr => mr.RecipeId == receita.Id)
                    .ToListAsync();
                dbContext.Tb_Materials_Recipe.RemoveRange(existingMaterials);

                foreach (var materialName in receitaAtualizada.Materials)
                {
                    var existingMaterial = await dbContext.Tb_Materials.FirstOrDefaultAsync(m => m.Name == materialName);
                    if (existingMaterial == null)
                    {
                        existingMaterial = new MaterialsModel { Name = materialName };
                        dbContext.Tb_Materials.Add(existingMaterial);
                        await dbContext.SaveChangesAsync(); // Salva o novo material no banco de dados
                    }
                    var materialRecipe = new MaterialRecipeModel
                    {
                        MaterialId = existingMaterial.Id,
                        RecipeId = receita.Id
                    };
                    dbContext.Tb_Materials_Recipe.Add(materialRecipe);
                }

                dbContext.Tb_Receita.Update(receita);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                response.response = receita;
                response.message = "Receita atualizada com sucesso!";
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                response.message = $"Erro ao atualizar receita: {e.Message}";
            }

            return response;
        }


        public async Task<DTOResponse> List_Recipe_By_ID(int id)
        {
            DTOResponse response = new DTOResponse();
            try
            {
                var query = await dbContext.Tb_Receita
                    .Where(receita => receita.Id == id)
                 .Select(receita => new
                 {
                     receita.Id,
                     receita.Title,
                     receita.Description,
                     receita.Instructions,
                     receita.ImageURL,
                     Admin = dbContext.Tb_User
                            .Where(user => user.Id == receita.UserId)
                            .Select(user => user.First_Name + " " + user.Last_Name)
                            .FirstOrDefault(),

                     // Buscar Categorias
                     Categorias = dbContext.Tb_RecipeCategory
                            .Where(rc => rc.RecipeId == receita.Id)
                            .Join(dbContext.Tb_Categoria,
                                  rc => rc.CategoryId,
                                  categoria => categoria.Id,
                                  (rc, categoria) => categoria.Name)
                            .ToList(),

                     // Buscar Ingredientes
                     Ingredients = dbContext.Tb_Ingredients_Recipe
                            .Where(ri => ri.RecipeId == receita.Id)
                            .Join(dbContext.Tb_Ingredients,
                            ri => ri.IngredientId,
                            ingrediente => ingrediente.Id,
                            (ri, ingrediente) => ingrediente.Name)
                            .ToList(),

                     // Buscar Materiais

                     Materials = dbContext.Tb_Materials_Recipe
                            .Where(rm => rm.RecipeId == receita.Id)
                            .Join(dbContext.Tb_Materials,
                             rm => rm.MaterialId,
                             material => material.Id,
                             (rm, material) => material.Name)
                             .ToList(),

                     Avaliacoes = dbContext.Tb_Receita_Avaliacao
                            .Where(ra => ra.RecipeId == receita.Id)
                            .Join(dbContext.Tb_Avaliacao,
                                  ra => ra.RatingId,
                                  avaliacao => avaliacao.Id,
                                  (ra, avaliacao) => new
                                  {
                                      avaliacao.Id,
                                      avaliacao.Value,
                                      avaliacao.Message,
                                      User = dbContext.Tb_User
                                          .Where(user => user.Id == avaliacao.Id_User)
                                          .Select(user => new
                                          {
                                              user.Id,
                                              user.First_Name,
                                              user.Last_Name,
                                              user.Email
                                          })
                                          .FirstOrDefault()
                                  })
                            .ToList()

                 })
                    .ToListAsync();


                response.response = query;
                response.message = "Sucess !";
            }
            catch (System.Exception e)
            {

                response.message = $"Opps we have a problem {e.Message}";
            }
            return response;
        }

        public async Task<DTOResponse> List_Recipe()
        {
            DTOResponse response = new DTOResponse();

            try
            {
                var query = await dbContext.Tb_Receita
                    .Select(receita => new
                    {
                        receita.Id,
                        receita.Title,
                        receita.Description,
                        receita.Instructions,
                        receita.ImageURL,
                        IdAdmin = dbContext.Tb_User
                            .Where(user => user.Id == receita.UserId)
                            .Select(user => user.Id)
                            .FirstOrDefault(),
                            
                        Admin = dbContext.Tb_User
                            .Where(user => user.Id == receita.UserId)
                            .Select(user => user.First_Name + " " + user.Last_Name)

                            .FirstOrDefault(),

                        // Buscar Categorias
                        Categorias = dbContext.Tb_RecipeCategory
                            .Where(rc => rc.RecipeId == receita.Id)
                            .Join(dbContext.Tb_Categoria,
                                  rc => rc.CategoryId,
                                  categoria => categoria.Id,
                                  (rc, categoria) => categoria.Name)
                            .ToList(),

                        // Buscar Ingredientes
                        Ingredients = dbContext.Tb_Ingredients_Recipe
                            .Where(ri => ri.RecipeId == receita.Id)
                            .Join(dbContext.Tb_Ingredients,
                            ri => ri.IngredientId,
                            ingrediente => ingrediente.Id,
                            (ri, ingrediente) => ingrediente.Name)
                            .ToList(),

                        // Buscar Materiais

                        Materials = dbContext.Tb_Materials_Recipe
                            .Where(rm => rm.RecipeId == receita.Id)
                            .Join(dbContext.Tb_Materials,
                             rm => rm.MaterialId,
                             material => material.Id,
                             (rm, material) => material.Name)
                             .ToList(),

                        Avaliacoes = dbContext.Tb_Receita_Avaliacao
                            .Where(ra => ra.RecipeId == receita.Id)
                            .Join(dbContext.Tb_Avaliacao,
                                  ra => ra.RatingId,
                                  avaliacao => avaliacao.Id,
                                  (ra, avaliacao) => new
                                  {
                                      avaliacao.Id,
                                      avaliacao.Value,
                                      avaliacao.Message,
                                      User = dbContext.Tb_User
                                          .Where(user => user.Id == avaliacao.Id_User)
                                          .Select(user => new
                                          {
                                              user.Id,
                                              user.First_Name,
                                              user.Last_Name,
                                              user.Email
                                          })
                                          .FirstOrDefault()
                                  })
                            .ToList()

                    })
                    .ToListAsync();

                response.response = query;
                response.message = "Sucesso";
            }
            catch (Exception e)
            {
                response.message = $"Alguma coisa deu errado: {e.Message}";
            }

            return response;
        }


        public async Task<DTOResponse> Delete_Recipe(int recipeId)
        {
            DTOResponse response = new DTOResponse();
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var recipe = await dbContext.Tb_Receita.FindAsync(recipeId);
                if (recipe == null)
                {
                    response.message = "Receita not found";
                    return response;
                }

                // Se a receita tiver uma imagem associada, exclua-a
                if (!string.IsNullOrEmpty(recipe.ImageURL))
                {
                    var imagePath = Path.Combine("Uploads", "Recipes", recipe.ImageURL);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                // Remove a receita do banco de dados
                dbContext.Tb_Receita.Remove(recipe);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                response.message = "Recipe deleted successfully";
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                response.message = $"Opps we have a problem {e.Message}";
            }

            return response;
        }
        public async Task<DTOResponse> Create_Recipe(RecipeModel receita, IFormFile? imageFile)
        {
            DTOResponse response = new DTOResponse();
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                // Salva a imagem da receita, se fornecida
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine("wwwroot", "uploads", "recipes");
                    var uniqueFileName = DateTime.Now.Ticks + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Criar o diretório se não existir
                    Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    receita.ImageURL = Path.Combine("uploads", "recipes", uniqueFileName);
                }

                dbContext.Tb_Receita.Add(receita);
                await dbContext.SaveChangesAsync();

                // Adiciona as categorias associadas
                foreach (var categoriaId in receita.Categorias)
                {
                    var receitaCategoria = new Recipe_CategoryModel
                    {
                        CategoryId = categoriaId,
                        RecipeId = receita.Id
                    };
                    dbContext.Tb_RecipeCategory.Add(receitaCategoria);
                }

                // Adiciona os ingredientes associados
                foreach (var ingredientName in receita.Ingredients)
                {
                    var existingIngredient = await dbContext.Tb_Ingredients.FirstOrDefaultAsync(i => i.Name == ingredientName);
                    if (existingIngredient == null)
                    {
                        existingIngredient = new IngredientsModel { Name = ingredientName };
                        dbContext.Tb_Ingredients.Add(existingIngredient);
                        await dbContext.SaveChangesAsync(); // Salva o novo ingrediente no banco de dados
                    }
                    var ingredientRecipe = new IngredientRecipeModel
                    {
                        IngredientId = existingIngredient.Id,
                        RecipeId = receita.Id
                    };
                    dbContext.Tb_Ingredients_Recipe.Add(ingredientRecipe);
                }

                // Adiciona os materiais associados
                foreach (var materialName in receita.Materials)
                {
                    var existingMaterial = await dbContext.Tb_Materials.FirstOrDefaultAsync(m => m.Name == materialName);
                    if (existingMaterial == null)
                    {
                        existingMaterial = new MaterialsModel { Name = materialName };
                        dbContext.Tb_Materials.Add(existingMaterial);
                        await dbContext.SaveChangesAsync(); // Salva o novo material no banco de dados
                    }
                    var materialRecipe = new MaterialRecipeModel
                    {
                        MaterialId = existingMaterial.Id,
                        RecipeId = receita.Id
                    };
                    dbContext.Tb_Materials_Recipe.Add(materialRecipe);
                }

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                response.response = receita;
                response.message = "Receita criada com sucesso!";
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                response.message = $"Erro ao criar receita: {e.Message}";
            }

            return response;
        }


        public async Task<DTOResponse> List_Recipe_By_User(int userId)
        {
            DTOResponse response = new DTOResponse();

            try
            {
                var query = await dbContext.Tb_Receita
                    .Where(receita => receita.UserId == userId)
                    .Select(receita => new
                    {
                        receita.Id,
                        receita.Title,
                        receita.Description,
                        receita.Instructions,
                        receita.ImageURL,
                        Admin = dbContext.Tb_User
                            .Where(user => user.Id == receita.UserId)
                            .Select(user => user.First_Name + " " + user.Last_Name)
                            .FirstOrDefault(),

                        // Buscar Categorias
                        // Buscar Categorias
                        Categorias = dbContext.Tb_RecipeCategory
                            .Where(rc => rc.RecipeId == receita.Id)
                            .Join(dbContext.Tb_Categoria,
                                  rc => rc.CategoryId,
                                  categoria => categoria.Id,
                                  (rc, categoria) => categoria.Name)
                            .ToList(),

                        // Buscar Ingredientes
                        Ingredients = dbContext.Tb_Ingredients_Recipe
                            .Where(ri => ri.RecipeId == receita.Id)
                            .Join(dbContext.Tb_Ingredients,
                            ri => ri.IngredientId,
                            ingrediente => ingrediente.Id,
                            (ri, ingrediente) => ingrediente.Name)
                            .ToList(),

                        // Buscar Materiais
                        Materials = dbContext.Tb_Materials_Recipe
                            .Where(rm => rm.RecipeId == receita.Id)
                            .Join(dbContext.Tb_Materials,
                             rm => rm.MaterialId,
                             material => material.Id,
                             (rm, material) => material.Name)
                             .ToList(),

                        Avaliacoes = dbContext.Tb_Receita_Avaliacao
                            .Where(ra => ra.RecipeId == receita.Id)
                            .Join(dbContext.Tb_Avaliacao,
                                  ra => ra.RatingId,
                                  avaliacao => avaliacao.Id,
                                  (ra, avaliacao) => new
                                  {
                                      avaliacao.Id,
                                      avaliacao.Value,
                                      avaliacao.Message,
                                      User = dbContext.Tb_User
                                          .Where(user => user.Id == avaliacao.Id_User)
                                          .Select(user => new
                                          {
                                              user.Id,
                                              user.First_Name,
                                              user.Last_Name,
                                              user.Email
                                          })
                                          .FirstOrDefault()
                                  })
                            .ToList()

                    })
                    .ToListAsync();

                response.response = query;
                response.message = "Success";
            }
            catch (Exception e)
            {
                response.message = $"Something went wrong: {e.Message}";
            }

            return response;
        }

        public async Task<DTOResponse> List_Recipe_By_Category(int categoryId)
        {
            DTOResponse response = new DTOResponse();

            try
            {
                var recipes = await dbContext.Tb_RecipeCategory
                    .Where(rc => rc.CategoryId == categoryId)
                    .Select(rc => rc.RecipeId)
                    .Distinct()
                    .ToListAsync();

                var recipeDetails = await dbContext.Tb_Receita
                    .Where(r => recipes.Contains(r.Id))
                    .Select(r => new
                    {
                        r.Id,
                        r.Title,
                        r.Description,
                        r.Instructions,
                        r.ImageURL,
                        Admin = dbContext.Tb_User
                            .Where(user => user.Id == r.UserId)
                            .Select(user => user.First_Name + " " + user.Last_Name)
                            .FirstOrDefault(),
                        Categorias = dbContext.Tb_RecipeCategory
                            .Where(rc => rc.RecipeId == r.Id)
                            .Join(dbContext.Tb_Categoria,
                                  rc => rc.CategoryId,
                                  categoria => categoria.Id,
                                  (rc, categoria) => categoria.Name)
                            .ToList(),
                        Ingredients = dbContext.Tb_Ingredients_Recipe
                            .Where(ri => ri.RecipeId == r.Id)
                            .Join(dbContext.Tb_Ingredients,
                                  ri => ri.IngredientId,
                                  ingrediente => ingrediente.Id,
                                  (ri, ingrediente) => ingrediente.Name)
                            .ToList(),
                        Materials = dbContext.Tb_Materials_Recipe
                            .Where(mr => mr.RecipeId == r.Id)
                            .Join(dbContext.Tb_Materials,
                                  mr => mr.MaterialId,
                                  material => material.Id,
                                  (mr, material) => material.Name)
                            .ToList(),
                        Avaliacoes = dbContext.Tb_Receita_Avaliacao
                            .Where(ra => ra.RecipeId == r.Id)
                            .Join(dbContext.Tb_Avaliacao,
                                  ra => ra.RatingId,
                                  avaliacao => avaliacao.Id,
                                  (ra, avaliacao) => new
                                  {
                                      avaliacao.Id,
                                      avaliacao.Value,
                                      avaliacao.Message,
                                      User = dbContext.Tb_User
                                          .Where(user => user.Id == avaliacao.Id_User)
                                          .Select(user => new
                                          {
                                              user.Id,
                                              user.First_Name,
                                              user.Last_Name,
                                              user.Email
                                          })
                                          .FirstOrDefault()
                                  })
                            .ToList()
                    })
                    .ToListAsync();

                response.response = recipeDetails;
                response.message = "Success";
            }
            catch (Exception e)
            {
                response.message = $"Something went wrong: {e.Message}";
            }

            return response;
        }


    }
}