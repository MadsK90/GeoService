namespace GeoService.Sdk;

public interface IGeoServiceApi
{
    #region Cabinets

    [Post(ApiRoutes.Cabinets.CreateCabinet)]
    Task<ApiResponse<CreateCabinetResponse>> CreateCabinetAsync(CreateCabinetRequest request);

    [GetWithReplaceId(ApiRoutes.Cabinets.GetCabinetById)]
    Task<ApiResponse<GetCabinetByIdResponse>> GetCabinetByIdAsync(GetCabinetByIdRequest request);

    [Put(ApiRoutes.Cabinets.UpdateCabinet)]
    Task<ApiResponse<UpdateCabinetResponse>> UpdateCabinetAsync(UpdateCabinetRequest request);

    [DeleteWithReplaceId(ApiRoutes.Cabinets.DeleteCabinet)]
    Task<ApiResponse<IResult>> DeleteCabinetAsync(DeleteCabinetRequest request);

    #endregion Cabinets

    #region Fibres

    [Post(ApiRoutes.Fibres.CreateFibre)]
    Task<ApiResponse<CreateFibreResponse>> CreateFibreAsync(CreateFibreRequest request);

    [GetWithReplaceId(ApiRoutes.Fibres.GetFibreById)]
    Task<ApiResponse<GetFibreByIdResponse>> GetFibreByIdAsync(GetFibreByIdRequest request);

    [Put(ApiRoutes.Fibres.UpdateFibre)]
    Task<ApiResponse<UpdateFibreResponse>> UpdateFibreAsync(UpdateFibreRequest request);

    [DeleteWithReplaceId(ApiRoutes.Fibres.DeleteFibre)]
    Task<ApiResponse<IResult>> DeleteFibreAsync(DeleteFibreRequest request);

    #endregion Fibres

    #region Manholes

    [Post(ApiRoutes.Manholes.CreateManhole)]
    Task<ApiResponse<CreateManholeResponse>> CreateManholeAsync(CreateManholeRequest request);

    [GetWithReplaceId(ApiRoutes.Manholes.GetManholeById)]
    Task<ApiResponse<GetManholeByIdResponse>> GetManholeByIdAsync(GetManholeByIdRequest request);

    [Put(ApiRoutes.Manholes.UpdateManhole)]
    Task<ApiResponse<UpdateManholeResponse>> UpdateManholeAsync(UpdateManholeRequest request);

    [DeleteWithReplaceId(ApiRoutes.Manholes.DeleteManhole)]
    Task<ApiResponse<IResult>> DeleteManholeAsync(DeleteManholeRequest request);

    #endregion Manholes

    #region Polygons

    [Post(ApiRoutes.Polygons.CreatePolygon)]
    Task<ApiResponse<CreatePolygonResponse>> CreatePolygonAsync(CreatePolygonRequest request);

    [GetWithReplaceId(ApiRoutes.Polygons.GetPolygonById)]
    Task<ApiResponse<GetPolygonByIdResponse>> GetPolygonByIdAsync(GetPolygonByIdRequest request);

    [Put(ApiRoutes.Polygons.UpdatePolygon)]
    Task<ApiResponse<UpdatePolygonResponse>> UpdatePolygonAsync(UpdatePolygonRequest request);

    [DeleteWithReplaceId(ApiRoutes.Polygons.DeletePolygon)]
    Task<ApiResponse<IResult>> DeletePolygonAsync(DeletePolygonRequest request);

    #endregion Polygons

    #region Routes

    [Post(ApiRoutes.Routes.CreateRoute)]
    Task<ApiResponse<CreateRouteResponse>> CreateRouteAsync(CreateRouteRequest request);

    [GetWithReplaceId(ApiRoutes.Routes.GetRouteById)]
    Task<ApiResponse<GetRouteByIdResponse>> GetRouteByIdAsync(GetRouteByIdRequest request);

    [Put(ApiRoutes.Routes.UpdateRoute)]
    Task<ApiResponse<UpdateRouteResponse>> UpdateRouteAsync(UpdateRouteRequest request);

    [DeleteWithReplaceId(ApiRoutes.Routes.DeleteRoute)]
    Task<ApiResponse<IResult>> DeleteRouteAsync(DeleteRouteRequest request);

    #endregion Routes

    #region Splitters

    [Post(ApiRoutes.Splitters.CreateSplitter)]
    Task<ApiResponse<CreateSplitterResponse>> CreateSplitterAsync(CreateSplitterRequest request);

    [GetWithReplaceId(ApiRoutes.Splitters.GetSplitterById)]
    Task<ApiResponse<GetSplitterByIdResponse>> GetSplitterByIdAsync(GetSplitterByIdRequest request);

    [Put(ApiRoutes.Splitters.UpdateSplitter)]
    Task<ApiResponse<UpdateSplitterResponse>> UpdateSplitterAsync(UpdateSplitterRequest request);

    [DeleteWithReplaceId(ApiRoutes.Splitters.DeleteSplitter)]
    Task<ApiResponse<IResult>> DeleteSplitterAsync(DeleteSplitterRequest request);

    #endregion Splitters
}