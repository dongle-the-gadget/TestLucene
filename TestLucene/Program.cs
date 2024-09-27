// See https://aka.ms/new-console-template for more information
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Search.Grouping;
using Lucene.Net.Store;

using FSDirectory directory = FSDirectory.Open("LuceneIndex");
using DirectoryReader reader = DirectoryReader.Open(directory);
IndexSearcher searcher = new IndexSearcher(reader);

Query query = new PhraseQuery()
{
    new Term("name", "create"),
    new Term("name", "file")
};

var docs = searcher.Search(query, 53);
var hasRequiredDoc = docs.ScoreDocs.Any(doc => searcher.Doc(doc.Doc).Get("name") == "CreateFileW");

GroupingSearch groupingSearch = new("name");
groupingSearch.SetAllGroups(true);

var groups = groupingSearch.Search(searcher, query, 0, 10);
bool hasRequiredGroup = false;

foreach (var group in groups.Groups)
{
    var firstDoc = group.ScoreDocs.First();
    var doc = searcher.Doc(firstDoc.Doc);

    if (doc.Get("name") == "CreateFileW")
    {
        hasRequiredGroup = true;
        break;
    }
}

Console.WriteLine($"{hasRequiredDoc} {hasRequiredGroup}");