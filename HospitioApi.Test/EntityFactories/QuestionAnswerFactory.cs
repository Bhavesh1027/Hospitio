using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Test.EntityFactories
{
    public class QuestionAnswerFactory
    {
        private readonly Faker<QuestionAnswer> _faker;
        public QuestionAnswerFactory()
        {
            _faker = new Faker<QuestionAnswer>()
                .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
                .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
                .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
                .RuleFor(m => m.IsActive, true)
                .RuleFor(m => m.QuestionAnswerCategoryId, f=>f.Random.Int(1, 999))
                .RuleFor(m => m.Name, f => f.Lorem.Word())
                .RuleFor(m => m.Description, f => f.Lorem.Sentence())
                .RuleFor(m => m.Icon, f => f.Lorem.Word())
                .RuleFor(m => m.IsPublish, true);

        }

        public QuestionAnswer SeedSingle(ApplicationDbContext db, int? QuestionAnswerCategoryId,bool? IsActive= true)
        {
            var questionAnswer = _faker.Generate();
            questionAnswer.QuestionAnswerCategoryId = QuestionAnswerCategoryId;
            questionAnswer.IsActive = IsActive;
            db.QuestionAnswers.Add(questionAnswer);
            db.SaveChanges();
            return questionAnswer;
        }

        public List<QuestionAnswer> SeedMany(ApplicationDbContext db, int QuestionAnswerCategoryId, int numberOfEntitiesToCreate)
        {
            var questionAnswers = Generate(QuestionAnswerCategoryId,numberOfEntitiesToCreate);
            db.QuestionAnswers.AddRange(questionAnswers);
            db.SaveChanges();
            return questionAnswers;
        }

        private List<QuestionAnswer> Generate(int QuestionAnswerCategoryId, int numberOfEntitiesCreate)
        {
            var faker = _faker.Clone()
                       .RuleFor(m => m.QuestionAnswerCategoryId, QuestionAnswerCategoryId);
            return faker.Generate(numberOfEntitiesCreate);
        }

    }
}
