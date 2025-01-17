﻿using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SharpBucket.V2;
using SharpBucket.V2.EndPoints;
using Shouldly;

namespace SharpBucketTests.V2.EndPoints
{
    [TestFixture]
    class UsersEndPointTests
    {
        private SharpBucketV2 sharpBucket;
        private UsersEndpoint usersEndPoint;

        [SetUp]
        public void Init()
        {
            sharpBucket = TestHelpers.SharpBucketV2;
            usersEndPoint = sharpBucket.UsersEndPoint(SampleRepositories.MIRROR_ACCOUNT_UUID);
        }

        [Test]
        public void GetProfile_FromMirrorAccount_ShouldReturnTheMirrorProfile()
        {
            usersEndPoint.ShouldNotBe(null);
            var profile = usersEndPoint.GetProfile();

            profile.uuid.ShouldNotBeNullOrEmpty(nameof(profile.uuid));
            profile.account_id.ShouldNotBeNullOrEmpty(nameof(profile.account_id));
            profile.nickname.ShouldBe("mirror", nameof(profile.nickname));
            profile.type.ShouldBe("user", nameof(profile.type));
            profile.display_name.ShouldBe("mirror", nameof(profile.display_name));
            profile.created_on.ShouldBe("2008-06-26T13:58:38+00:00");
            profile.account_status.ShouldBe("active", nameof(profile.account_status));
        }

        [Test]
        public async Task GetProfileAsync_FromMirrorAccount_ShouldReturnTheMirrorProfile()
        {
            usersEndPoint.ShouldNotBe(null);
            var profile = await usersEndPoint.GetProfileAsync();

            profile.uuid.ShouldNotBeNullOrEmpty(nameof(profile.uuid));
            profile.account_id.ShouldNotBeNullOrEmpty(nameof(profile.account_id));
            profile.nickname.ShouldBe("mirror", nameof(profile.nickname));
            profile.type.ShouldBe("user", nameof(profile.type));
            profile.display_name.ShouldBe("mirror", nameof(profile.display_name));
            profile.created_on.ShouldBe("2008-06-26T13:58:38+00:00");
            profile.account_status.ShouldBe("active", nameof(profile.account_status));
        }

        [Test]
        [Obsolete("Test of an obsolete method")]
        public void ListRepositories_FromMirrorAccount_ShouldReturnMirrorsRepositories()
        {
            usersEndPoint.ShouldNotBe(null);
            var repositories = usersEndPoint.ListRepositories();
            repositories.Count.ShouldBeGreaterThan(10);
            repositories = usersEndPoint.ListRepositories(max: 5);
            repositories.Count.ShouldBe(5);
        }
    }
}