using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Test.EntityFactories
{
    public class QuestionAnswerCategoryFactory
    {
        private readonly Faker<QuestionAnswerCategory> _faker;
        public QuestionAnswerCategoryFactory()
        {
            _faker = new Faker<QuestionAnswerCategory>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.IsActive, true)
                .RuleFor(m => m.Name, f => f.Lorem.Word());
               
        }

        public QuestionAnswerCategory SeedSingle(ApplicationDbContext db)
        {
            var QaCategory = _faker.Generate();
            db.QuestionAnswerCategories.Add(QaCategory);
            db.SaveChanges();
            return QaCategory;
        }

        public List<QuestionAnswerCategory> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
        {
            var qaCategories = _faker.Generate(numberOfEntitiesToCreate);
            db.QuestionAnswerCategories.AddRange(qaCategories);
            db.SaveChanges();
            return qaCategories;
        }
    }
}
