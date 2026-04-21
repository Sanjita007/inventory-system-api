using accswift_api.Controllers;
using inventory_system_api.Application.IRepository.Invenetory;
using inventory_system_api.Application.Models;
using inventory_system_api.Application.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Assert = Xunit.Assert;

namespace inventory_system_api.unitTests
{
    public class ProductGroupControllerTests
    {
        private readonly Mock<IProductGroupRepository> _mockRepo;
        private readonly ProductGroupController _controller;

        public ProductGroupControllerTests()
        {
            _mockRepo = new Mock<IProductGroupRepository>();
            _controller = new ProductGroupController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenProductGroupExists()
        {
            // Arrange
            var groupId = 1;
            var fakeGroup = new ProductGroup
            {
                ID = groupId,
                ParentGroupID = 12,
                EngName = "Test Group",
                NepName = "टेस्ट समूह",
                Level = 1,
                ParentGroupName = "Parent Group",
                Remarks = "This is a test product group"

            };

            _mockRepo.Setup(repo => repo.Get(groupId)).ReturnsAsync(fakeGroup);

            // Act
            var result = await _controller.Get(groupId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var respo = Assert.IsType<Models.Response>(okResult.Value);
            var returnedGroup = Assert.IsType<ProductGroup>(respo.Data);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(200, respo.StatusCode);
            Assert.Equal("Success", respo.Message);
            Assert.Equal("Test Group", returnedGroup.EngName);
        }

        [Fact]
        public async Task AddEdit_ReturnsOk_OnSuccess()
        {
            int groupId = 1;
            // Arrange
            var newGroup = new ProductGroup
            {
                ID = groupId,
                ParentGroupID = 12,
                EngName = "Test Group",
                NepName = "टेस्ट समूह",
                Level = 1,
                ParentGroupName = "Parent Group",
                Remarks = "This is a test product group"
            };

            _mockRepo.Setup(repo => repo.AddEdit(It.IsAny<ProductGroup>())).ReturnsAsync(1);

            // Act
            var result = await _controller.Post(newGroup);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<Models.Response>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode); // HTTP response
            Assert.Equal(200, returned.StatusCode); // custom response
            Assert.Equal("Success", returned.Message);
            Assert.NotNull(returned.Data);
            _mockRepo.Verify(repo => repo.AddEdit(It.IsAny<ProductGroup>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenProductGroupIsDeleted()
        {
            // Arrange
            int groupIdToDelete = 10;

            _mockRepo.Setup(repo => repo.Delete(groupIdToDelete)).ReturnsAsync(1);

            // Act
            var result = await _controller.Delete(groupIdToDelete);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Models.Response>(okResult.Value);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal("Success", response.Message);
            Assert.Null(response.Data);

            _mockRepo.Verify(repo => repo.Delete(groupIdToDelete), Times.Once);
        }

        [Fact]
        public async Task GetTree_ReturnsHierarchy_WhenProductGroupsExist()
        {
            // Arrange - make a small Tree with childrenand grandchildren 
            var expectedTree = new List<Tree>
            {
                new() {
                    Id = 1,
                    Name = "Root",
                    ParentID = 0,
                    Level = 0,
                    Children =
                    [
                        new Tree
                        {
                            Id = 2,
                            Name = "Child A",
                            ParentID = 1,
                            Level = 1,
                            Children =
                            [
                                new Tree { Id = 4, Name = "Grandchild", ParentID = 2, Level = 2, Children = new List<Tree>() }
                            ]
                        },
                        new Tree { Id = 3, Name = "Child B", ParentID = 1, Level = 1, Children = new List<Tree>() }
                    ]
                }
            };

            _mockRepo.Setup(r => r.GetProductTrees()).ReturnsAsync(expectedTree);

            // Act
            var actionResult = await _controller.GetTree();

            // Assert - HTTP shape and payload
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var response = Assert.IsType<Models.Response>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(200, response.StatusCode);
            Assert.Equal("Success", response.Message);

            var tree = Assert.IsType<List<Tree>>(response.Data);

            // there should be one root in the returned tree
            Assert.Single(tree);
            var root = tree.First();
            Assert.Equal("Root", root.Name);

            // root should have two children (Child A and Child B)
            Assert.Equal(2, root.Children.Count);
            var childA = root.Children.First(c => c.Name == "Child A");
            var childB = root.Children.First(c => c.Name == "Child B");

            // Child A should have one child (Grandchild)
            Assert.Single(childA.Children);
            Assert.Equal("Grandchild", childA.Children[0].Name);

            // Child B should have no children
            Assert.Empty(childB.Children);

            _mockRepo.Verify(r => r.GetProductTrees(), Times.Once);
        }
    }

}
