using Application.Controllers.SocialReview.Extensions;
using Newtonsoft.Json;
using System.Dynamic;
using Xunit;

namespace ReadingGraphAPIs.Tests
{
    public class DynamicExtensionsTests
    {
        private string _stringApiResult=string.Empty;
        
        public DynamicExtensionsTests()
        {
            string projectDirectory =
                Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName!;

            var stringApiResultPath = $"{projectDirectory}\\stringApiResult.json";

            using StreamReader r = new StreamReader(stringApiResultPath);
            _stringApiResult = r.ReadToEnd();
        }

        [Fact]
        public void TryExtractValueShouldReturnTrueAndIdGivenCorrectNonNestedPathToId()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.TryExtractValue
                (dynamicApiResult, "id", out string idValue);

            //Assert
            Assert.True(operationResult);
            Assert.Equal("105063339084349", idValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnFalseAndNullGivenWrongNonNestedPathToId()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.TryExtractValue
                (dynamicApiResult, "id1", out string idValue);

            //Assert
            Assert.False(operationResult);
            Assert.Null(idValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnTrueAndNameGivenCorrectNestedPathToName()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.TryExtractValue
                (dynamicApiResult, "comments.data[1].from.name", out string  nameValue);

            //Assert
            Assert.True(operationResult);
            Assert.Equal("Will Smith", nameValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnFalseAndNullGivenWrongNestedPathToName()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.TryExtractValue
                (dynamicApiResult, "comments.data[1].from.name1", out string nameValue);

            //Assert
            Assert.False(operationResult);
            Assert.Null(nameValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnTrueAndRecommendationTypeGivenCorrectNestedPathToRecommendationType()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.TryExtractValue
                (dynamicApiResult, "data.recommendation_type", out string recommendationTypeValue);

            //Assert
            Assert.True(operationResult);
            Assert.Equal("positive", recommendationTypeValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnFalseAndNullGivenWrongNestedPathToRecommendationType()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.TryExtractValue
                (dynamicApiResult, "data.recommendation_type1", out string recommendationTypeValue);

            //Assert
            Assert.False(operationResult);
            Assert.Null( recommendationTypeValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnTrueAndBeforeGivenCorrectNestedPathToBefore()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.TryExtractValue
                (dynamicApiResult, "comments.paging.cursors.before", out string beforeValue);

            //Assert
            Assert.True(operationResult);
            Assert.Contains("QVFIUkQ3Y0VuS0IzZA", beforeValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnFalseAndNullGivenWrongNestedPathToBefore()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.TryExtractValue
                (dynamicApiResult, "comments.paging.cursors.before1", out string beforeValue);

            //Assert
            Assert.False(operationResult);
            Assert.Null(beforeValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnTrueAndCommentGivenCorrectNestedPathToFirstComment()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.TryExtractValue
                (dynamicApiResult, "comments.data[0]", out Comment comment);

            //Assert
            Assert.True(operationResult);
            Assert.IsType<Comment>(comment);
            Assert.Equal("105063339084349_801664927626415", comment.Id);
            Assert.Equal("This is John's first comment.", comment.Message);
            Assert.Equal(new DateTime(2022,11,3).Date, comment.CreatedTime.Date);
            Assert.Equal(13, comment.CommentCount);
        }
        [Fact]
        public void TryExtractValueShouldReturnTrueAndCommentGivenCorrectNestedPathToSecondComment()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.TryExtractValue
                (dynamicApiResult, "comments.data[1]", out Comment comment);

            //Assert
            Assert.True(operationResult);
            Assert.IsType<Comment>(comment);
            Assert.Equal("105063339084349_801664927626416", comment.Id);
            Assert.Equal("This is Will's first comment.", comment.Message);
            Assert.Equal(new DateTime(2022, 11, 4).Date, comment.CreatedTime.Date);
            Assert.Equal(7, comment.CommentCount);
        }
        [Fact]
        public void TryExtractValueShouldReturnFalseGivenWrongtNestedPathToThirdComment()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;
            //Act
            var operationResult = DynamicExtensions.TryExtractValue
                (dynamicApiResult, "comments.data[2]", out string beforeValue);

            //Assert
            Assert.False(operationResult);
            Assert.Null(beforeValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnCommentListGivenCorrectNestedPathToComments()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.TryExtractValue
                (dynamicApiResult, "comments.data", out List<Comment> comments);

            //Assert
            Assert.True(operationResult);
            Assert.IsType<List<Comment>>(comments);

            Assert.Equal("105063339084349_801664927626415", comments[0].Id);
            Assert.Equal("This is John's first comment.", comments[0].Message);
            Assert.Equal(new DateTime(2022, 11, 3).Date, comments[0].CreatedTime.Date);
            Assert.Equal(13, comments[0].CommentCount);

            Assert.Equal("105063339084349_801664927626416", comments[1].Id);
            Assert.Equal("This is Will's first comment.", comments[1].Message);
            Assert.Equal(new DateTime(2022, 11, 4).Date, comments[1].CreatedTime.Date);
            Assert.Equal(7, comments[1].CommentCount);
        }


        [Fact]
        public void ExtractValueShouldReturnIdGivenCorrectNonNestedPathToId()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var idValue = DynamicExtensions.ExtractValue<string>
                (dynamicApiResult, "id");

            //Assert
            Assert.Equal("105063339084349", idValue);
        }
        [Fact]
        public void ExtractValueShouldReturnExceptionGivenWrongNonNestedPathToId()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            Action action = () => DynamicExtensions.ExtractValue<string>
                (dynamicApiResult, "id1");

            //Assert
            var exception = Assert.Throws<InvalidOperationException>(action);
            Assert.Contains("field name not found", exception.Message);
        }
        [Fact]
        public void ExtractValueShouldReturnNameGivenCorrectNestedPathToName()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var nameValue = DynamicExtensions.ExtractValue<string>
                (dynamicApiResult, "comments.data[1].from.name");

            //Assert
            Assert.Equal("Will Smith", nameValue);
        }
        [Fact]
        public void ExtractValueShouldReturnExceptionGivenWrongNestedPathToName()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            Action action = () => DynamicExtensions.ExtractValue<string>
                (dynamicApiResult, "comments.data[1].from.name1");

            //Assert
            var exception = Assert.Throws<InvalidOperationException>(action);
            Assert.Contains("field name not found", exception.Message);
        }
        [Fact]
        public void ExtractValueShouldReturnRecommendationTypeGivenCorrectNestedPathToRecommendationType()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var recommendationTypeValue = DynamicExtensions.ExtractValue<string>
                (dynamicApiResult, "data.recommendation_type");

            //Assert
            Assert.Equal("positive", recommendationTypeValue);
        }
        [Fact]
        public void ExtractValueShouldReturnExceptionGivenWrongNestedPathToRecommendationType()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            Action action = () => DynamicExtensions.ExtractValue<string>
                (dynamicApiResult, "data.recommendation_type1");

            //Assert
            var exception = Assert.Throws<InvalidOperationException>(action);
            Assert.Contains("field name not found", exception.Message);
        }
        [Fact]
        public void ExtractValueShouldReturnBeforeGivenCorrectNestedPathToBefore()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var beforeValue = DynamicExtensions.ExtractValue<string>
                (dynamicApiResult, "comments.paging.cursors.before");

            //Assert
            Assert.Contains("QVFIUkQ3Y0VuS0IzZA", beforeValue);
        }
        [Fact]
        public void ExtractValueShouldReturnExceptionGivenWrongNestedPathToBefore()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            Action action = () => DynamicExtensions.ExtractValue<string>
                (dynamicApiResult, "comments.paging.cursors.before1");

            //Assert
            var exception = Assert.Throws<InvalidOperationException>(action);
            Assert.Contains("field name not found", exception.Message);
        }
        [Fact]
        public void ExtractValueShouldReturnCommentGivenCorrectNestedPathToFirstComment()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var comment = DynamicExtensions.ExtractValue<Comment>
                (dynamicApiResult, "comments.data[0]");

            //Assert
            Assert.IsType<Comment>(comment);
            Assert.Equal("105063339084349_801664927626415", comment.Id);
            Assert.Equal("This is John's first comment.", comment.Message);
            Assert.Equal(new DateTime(2022, 11, 3).Date, comment.CreatedTime.Date);
            Assert.Equal(13, comment.CommentCount);
        }
        [Fact]
        public void ExtractValueShouldReturnCommentGivenCorrectNestedPathToSecondComment()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var comment = DynamicExtensions.ExtractValue<Comment>
                (dynamicApiResult, "comments.data[1]");

            //Assert
            Assert.IsType<Comment>(comment);
            Assert.Equal("105063339084349_801664927626416", comment.Id);
            Assert.Equal("This is Will's first comment.", comment.Message);
            Assert.Equal(new DateTime(2022, 11, 4).Date, comment.CreatedTime.Date);
            Assert.Equal(7, comment.CommentCount);
        }
        [Fact]
        public void ExtractValueShouldReturnExceptionGivenWrongtNestedPathToThirdComment()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            Action action = () => DynamicExtensions.ExtractValue<Comment>
                (dynamicApiResult, "comments.data[2]");

            //Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(action);
            Assert.Contains("Index was out of range", exception.Message);
        }
        [Fact]
        public void ExtractValueShouldReturnCommentListGivenCorrectNestedPathToComments()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var comments = DynamicExtensions.ExtractValue<List<Comment>>
                (dynamicApiResult, "comments.data");

            //Assert            
            Assert.IsType<List<Comment>>(comments);

            Assert.Equal("105063339084349_801664927626415", comments[0].Id);
            Assert.Equal("This is John's first comment.", comments[0].Message);
            Assert.Equal(new DateTime(2022, 11, 3).Date, comments[0].CreatedTime.Date);
            Assert.Equal(13, comments[0].CommentCount);

            Assert.Equal("105063339084349_801664927626416", comments[1].Id);
            Assert.Equal("This is Will's first comment.", comments[1].Message);
            Assert.Equal(new DateTime(2022, 11, 4).Date, comments[1].CreatedTime.Date);
            Assert.Equal(7, comments[1].CommentCount);
        }


        [Fact]
        public void HasValueShouldReturnTrueGivenCorrectNonNestedPathToId()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.HasValue
                (dynamicApiResult, "id");

            //Assert
            Assert.True(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnFalseGivenWrongNonNestedPathToId()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.HasValue
                (dynamicApiResult, "id1");

            //Assert
            Assert.False(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnTrueGivenCorrectNestedPathToName()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.HasValue
                (dynamicApiResult, "comments.data[1].from.name");

            //Assert
            Assert.True(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnFalseGivenWrongNestedPathToName()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.HasValue
                (dynamicApiResult, "comments.data[1].from.name1");

            //Assert
            Assert.False(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnTrueGivenCorrectNestedPathToRecommendationType()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.HasValue
                (dynamicApiResult, "data.recommendation_type");

            //Assert
            Assert.True(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnFalseGivenWrongNestedPathToRecommendationType()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.HasValue
                (dynamicApiResult, "data.recommendation_type1");

            //Assert
            Assert.False(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnTrueGivenCorrectNestedPathToBefore()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.HasValue
                (dynamicApiResult, "comments.paging.cursors.before");

            //Assert
            Assert.True(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnFalseGivenWrongNestedPathToBefore()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.HasValue
                (dynamicApiResult, "comments.paging.cursors.before!");

            //Assert
            Assert.False(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnTrueGivenCorrectNestedPathToFirstComment()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.HasValue
                (dynamicApiResult, "comments.data[0]");

            //Assert
            Assert.True(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnTruetGivenCorrectNestedPathToSecondComment()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.HasValue
                (dynamicApiResult, "comments.data[1]");

            //Assert
            Assert.True(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnFalseGivenWrongtNestedPathToThirdComment()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.HasValue
                (dynamicApiResult, "comments.data[2]");

            //Assert
            Assert.False(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnTrueGivenCorrectNestedPathToComments()
        {
            //Arrange
            var dynamicApiResult =
                JsonConvert.DeserializeObject<ExpandoObject>(_stringApiResult)!;

            //Act
            var operationResult = DynamicExtensions.HasValue
                (dynamicApiResult, "comments.data");

            //Assert
            Assert.True(operationResult);
        }
    }
}