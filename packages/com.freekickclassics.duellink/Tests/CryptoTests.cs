using System;
using System.Text;
using FreeKickClassics.DuelLink.Crypto;
using NUnit.Framework;

namespace FreeKickClassics.DuelLink.Tests
{
    public class Sha256Tests
    {
        [TestCase("", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")]
        [TestCase("abc", "ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad")]
        [TestCase("abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq", "248d6a61d20638b8e5c026930c3e6039a33ce45964ff2167f6ecedd419db06c1")]
        public void MatchesPublishedVectors(string input, string expectedHex)
        {
            Assert.That(Hex(Sha256.Hash(Encoding.ASCII.GetBytes(input))), Is.EqualTo(expectedHex));
        }

        [Test]
        public void MatchesTheFrameworkImplementationAcrossBlockBoundaries()
        {
            var random = new Random(1234);
            using (var reference = System.Security.Cryptography.SHA256.Create())
            {
                for (int length = 0; length <= 200; length++)
                {
                    var data = new byte[length];
                    random.NextBytes(data);
                    Assert.That(Sha256.Hash(data), Is.EqualTo(reference.ComputeHash(data)), "length " + length);
                }
            }
        }

        static string Hex(byte[] bytes)
        {
            var builder = new StringBuilder();
            foreach (byte b in bytes) builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }

    public class CommitmentTests
    {
        static readonly byte[] SessionId = { 1, 2, 3, 4, 5, 6, 7, 8 };
        static readonly byte[] Payload = { 3, 1, 200, 0 };
        static readonly byte[] Nonce = { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0, 11, 12 };

        [Test]
        public void IsSixteenBytesAndVerifies()
        {
            var commitment = Commitment.Compute(SessionId, 3, DuelRole.Host, Payload, Nonce);
            Assert.That(commitment.Length, Is.EqualTo(Commitment.Size));
            Assert.That(Commitment.Verify(commitment, SessionId, 3, DuelRole.Host, Payload, Nonce), Is.True);
        }

        [Test]
        public void RejectsAChangedPick()
        {
            var commitment = Commitment.Compute(SessionId, 3, DuelRole.Host, Payload, Nonce);
            var changed = (byte[])Payload.Clone();
            changed[0] = 5;
            Assert.That(Commitment.Verify(commitment, SessionId, 3, DuelRole.Host, changed, Nonce), Is.False);
        }

        [Test]
        public void RejectsAChangedNonce()
        {
            var commitment = Commitment.Compute(SessionId, 3, DuelRole.Host, Payload, Nonce);
            var changed = (byte[])Nonce.Clone();
            changed[11] ^= 1;
            Assert.That(Commitment.Verify(commitment, SessionId, 3, DuelRole.Host, Payload, changed), Is.False);
        }

        [Test]
        public void IsBoundToWhoCommitted()
        {
            // A guest that copies the host's commitment can't later pass the host's reveal off as its own.
            var hostCommitment = Commitment.Compute(SessionId, 3, DuelRole.Host, Payload, Nonce);
            Assert.That(Commitment.Verify(hostCommitment, SessionId, 3, DuelRole.Guest, Payload, Nonce), Is.False);
        }

        [Test]
        public void IsBoundToSessionAndRound()
        {
            var commitment = Commitment.Compute(SessionId, 3, DuelRole.Host, Payload, Nonce);
            Assert.That(Commitment.Verify(commitment, SessionId, 4, DuelRole.Host, Payload, Nonce), Is.False);
            var otherSession = (byte[])SessionId.Clone();
            otherSession[0] = 99;
            Assert.That(Commitment.Verify(commitment, otherSession, 3, DuelRole.Host, Payload, Nonce), Is.False);
        }

        [Test]
        public void RejectsWrongSizes()
        {
            Assert.Throws<ArgumentException>(() => Commitment.Compute(SessionId, 0, DuelRole.Host, new byte[3], Nonce));
            Assert.Throws<ArgumentException>(() => Commitment.Compute(SessionId, 0, DuelRole.Host, Payload, new byte[8]));
            Assert.That(Commitment.Verify(new byte[15], SessionId, 3, DuelRole.Host, Payload, Nonce), Is.False);
        }
    }

    public class SharedRandomnessTests
    {
        [Test]
        public void SessionIdDependsOnBothNoncesInOrder()
        {
            var a = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 };
            var b = new byte[] { 2, 2, 2, 2, 2, 2, 2, 2 };
            Assert.That(SharedRandomness.SessionId(a, b).Length, Is.EqualTo(SharedRandomness.SessionIdSize));
            Assert.That(SharedRandomness.SessionId(a, b), Is.Not.EqualTo(SharedRandomness.SessionId(b, a)));
        }

        [Test]
        public void CoinTossIsRoughlyFair()
        {
            var source = new InsecureSeededNonceSource(7);
            int heads = 0;
            var id = new byte[SharedRandomness.SessionIdSize];
            for (int i = 0; i < 2000; i++)
            {
                source.Fill(id);
                if (SharedRandomness.CoinToss(id)) heads++;
            }

            Assert.That(heads, Is.InRange(900, 1100));
        }

        [Test]
        public void RoundSeedChangesWithEveryInput()
        {
            var id = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var host = new byte[12];
            var guest = new byte[12];
            guest[0] = 1;
            ulong seed = SharedRandomness.RoundSeed(id, 0, host, guest);
            Assert.That(SharedRandomness.RoundSeed(id, 1, host, guest), Is.Not.EqualTo(seed));
            Assert.That(SharedRandomness.RoundSeed(id, 0, guest, host), Is.Not.EqualTo(seed));
            Assert.That(SharedRandomness.RoundSeed(id, 0, host, guest), Is.EqualTo(seed));
        }
    }
}
