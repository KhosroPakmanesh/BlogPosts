using Xunit;

namespace ReadingGraphAPIs.Tests
{
    public class StringExtensionsTests
    {
        private string _stringApiResult = string.Empty;

        public StringExtensionsTests()
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
            //Act
            var operationResult = _stringApiResult.TryExtractValue
                ( "id", out string idValue);

            //Assert
            Assert.True(operationResult);
            Assert.Equal("105063339084349", idValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnFalseAndNullGivenWrongNonNestedPathToId()
        {
            //Act
            var operationResult = _stringApiResult.TryExtractValue
                ("id1", out string idValue);

            //Assert
            Assert.False(operationResult);
            Assert.Null(idValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnTrueAndNameGivenCorrectNestedPathToName()
        {
            //Act
            var operationResult = _stringApiResult.TryExtractValue
                ("comments.data[1].from.name", out string nameValue);

            //Assert
            Assert.True(operationResult);
            Assert.Equal("Will Smith", nameValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnFalseAndNullGivenWrongNestedPathToName()
        {
            //Act
            var operationResult = _stringApiResult.TryExtractValue
                ("comments.data[1].from.name1", out string nameValue);

            //Assert
            Assert.False(operationResult);
            Assert.Null(nameValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnTrueAndRecommendationTypeGivenCorrectNestedPathToRecommendationType()
        {
            //Act
            var operationResult = _stringApiResult.TryExtractValue
                ("data.recommendation_type", out string recommendationTypeValue);

            //Assert
            Assert.True(operationResult);
            Assert.Equal("positive", recommendationTypeValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnFalseAndNullGivenWrongNestedPathToRecommendationType()
        {
            //Act
            var operationResult = _stringApiResult.TryExtractValue
                ("data.recommendation_type1", out string recommendationTypeValue);

            //Assert
            Assert.False(operationResult);
            Assert.Null(recommendationTypeValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnTrueAndBeforeGivenCorrectNestedPathToBefore()
        {
            //Act
            var operationResult = _stringApiResult.TryExtractValue
                ("comments.paging.cursors.before", out string beforeValue);

            //Assert
            Assert.True(operationResult);
            Assert.Contains("QVFIUkQ3Y0VuS0IzZA", beforeValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnFalseAndNullGivenWrongNestedPathToBefore()
        {
            //Act
            var operationResult = _stringApiResult.TryExtractValue
                ("comments.paging.cursors.before1", out string beforeValue);

            //Assert
            Assert.False(operationResult);
            Assert.Null(beforeValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnTrueAndCommentGivenCorrectNestedPathToFirstComment()
        {
            //Act
            var operationResult = _stringApiResult.TryExtractValue
                ("comments.data[0]", out Comment comment);

            //Assert
            Assert.True(operationResult);
            Assert.IsType<Comment>(comment);
            Assert.Equal("105063339084349_801664927626415", comment.Id);
            Assert.Equal("This is John's first comment.", comment.Message);
            Assert.Equal(new DateTime(2022, 11, 3).Date, comment.CreatedTime.Date);
            Assert.Equal(13, comment.CommentCount);
        }
        [Fact]
        public void TryExtractValueShouldReturnTrueAndCommentGivenCorrectNestedPathToSecondComment()
        {
            //Act
            var operationResult = _stringApiResult.TryExtractValue
                ("comments.data[1]", out Comment comment);

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
            //Act
            var operationResult = _stringApiResult.TryExtractValue
                ("comments.data[2]", out string beforeValue);

            //Assert
            Assert.False(operationResult);
            Assert.Null(beforeValue);
        }
        [Fact]
        public void TryExtractValueShouldReturnCommentListGivenCorrectNestedPathToComments()
        {
            //Act
            var operationResult = _stringApiResult.TryExtractValue
                ("comments.data", out List<Comment> comments);

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
            //Act
            var idValue = _stringApiResult.ExtractValue<string>("id");

            //Assert
            Assert.Equal("105063339084349", idValue);
        }
        [Fact]
        public void ExtractValueShouldReturnExceptionGivenWrongNonNestedPathToId()
        {
            //Act
            Action action = () => _stringApiResult.ExtractValue<string>("id1");

            //Assert
            var exception = Assert.Throws<InvalidOperationException>(action);
            Assert.Contains("field name not found", exception.Message);
        }
        [Fact]
        public void ExtractValueShouldReturnNameGivenCorrectNestedPathToName()
        {
            //Act
            var nameValue = _stringApiResult.ExtractValue<string>
                ("comments.data[1].from.name");

            //Assert
            Assert.Equal("Will Smith", nameValue);
        }
        [Fact]
        public void ExtractValueShouldReturnExceptionGivenWrongNestedPathToName()
        {
            //Act
            Action action = () => _stringApiResult.ExtractValue<string>
                ("comments.data[1].from.name1");

            //Assert
            var exception = Assert.Throws<InvalidOperationException>(action);
            Assert.Contains("field name not found", exception.Message);
        }
        [Fact]
        public void ExtractValueShouldReturnRecommendationTypeGivenCorrectNestedPathToRecommendationType()
        {
            //Act
            var recommendationTypeValue = _stringApiResult.ExtractValue<string>
                ("data.recommendation_type");

            //Assert
            Assert.Equal("positive", recommendationTypeValue);
        }
        [Fact]
        public void ExtractValueShouldReturnExceptionGivenWrongNestedPathToRecommendationType()
        {
            //Act
            Action action = () => _stringApiResult.ExtractValue<string>
                ("data.recommendation_type1");

            //Assert
            var exception = Assert.Throws<InvalidOperationException>(action);
            Assert.Contains("field name not found", exception.Message);
        }
        [Fact]
        public void ExtractValueShouldReturnBeforeGivenCorrectNestedPathToBefore()
        {
            //Act
            var beforeValue = _stringApiResult.ExtractValue<string>
                ("comments.paging.cursors.before");

            //Assert
            Assert.Contains("QVFIUkQ3Y0VuS0IzZA", beforeValue);
        }
        [Fact]
        public void ExtractValueShouldReturnExceptionGivenWrongNestedPathToBefore()
        {
            //Act
            Action action = () => _stringApiResult.ExtractValue<string>
                ("comments.paging.cursors.before1");

            //Assert
            var exception = Assert.Throws<InvalidOperationException>(action);
            Assert.Contains("field name not found", exception.Message);
        }
        [Fact]
        public void ExtractValueShouldReturnCommentGivenCorrectNestedPathToFirstComment()
        {
            //Act
            var comment = _stringApiResult
                .ExtractValue<Comment>("comments.data[0]");

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
            //Act
            var comment = _stringApiResult
                .ExtractValue<Comment>("comments.data[1]");

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
            //Act
            Action action = () => _stringApiResult
                .ExtractValue<Comment>("comments.data[2]");

            //Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(action);
            Assert.Contains("Index was out of range", exception.Message);
        }
        [Fact]
        public void ExtractValueShouldReturnCommentListGivenCorrectNestedPathToComments()
        {
            //Act
            var comments = _stringApiResult
                .ExtractValue<List<Comment>>("comments.data");

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
            //Act
            var operationResult = _stringApiResult.HasValue("id");

            //Assert
            Assert.True(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnFalseGivenWrongNonNestedPathToId()
        {
            //Act
            var operationResult = _stringApiResult.HasValue("id1");

            //Assert
            Assert.False(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnTrueGivenCorrectNestedPathToName()
        {
            //Act
            var operationResult = _stringApiResult
                .HasValue("comments.data[1].from.name");

            //Assert
            Assert.True(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnFalseGivenWrongNestedPathToName()
        {
            //Act
            var operationResult = _stringApiResult
                .HasValue("comments.data[1].from.name1");

            //Assert
            Assert.False(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnTrueGivenCorrectNestedPathToRecommendationType()
        {
            //Act
            var operationResult = _stringApiResult
                .HasValue("data.recommendation_type");

            //Assert
            Assert.True(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnFalseGivenWrongNestedPathToRecommendationType()
        {
            //Act
            var operationResult = _stringApiResult
                .HasValue("data.recommendation_type1");

            //Assert
            Assert.False(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnTrueGivenCorrectNestedPathToBefore()
        {
            //Act
            var operationResult = _stringApiResult
                .HasValue("comments.paging.cursors.before");

            //Assert
            Assert.True(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnFalseGivenWrongNestedPathToBefore()
        {
            //Act
            var operationResult = _stringApiResult
                .HasValue("comments.paging.cursors.before!");

            //Assert
            Assert.False(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnTrueGivenCorrectNestedPathToFirstComment()
        {
            //Act
            var operationResult = _stringApiResult
                .HasValue("comments.data[0]");

            //Assert
            Assert.True(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnTruetGivenCorrectNestedPathToSecondComment()
        {
            //Act
            var operationResult = _stringApiResult
                .HasValue("comments.data[1]");

            //Assert
            Assert.True(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnFalseGivenWrongtNestedPathToThirdComment()
        {
            //Act
            var operationResult = _stringApiResult
                .HasValue("comments.data[2]");

            //Assert
            Assert.False(operationResult);
        }
        [Fact]
        public void HasValueShouldReturnTrueGivenCorrectNestedPathToComments()
        {
            //Act
            var operationResult = _stringApiResult
                .HasValue("comments.data");

            //Assert
            Assert.True(operationResult);
        }
    }
}