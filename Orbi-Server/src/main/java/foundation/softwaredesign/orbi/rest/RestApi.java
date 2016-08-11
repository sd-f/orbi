package foundation.softwaredesign.orbi.rest;

import foundation.softwaredesign.orbi.model.real.Position;
import foundation.softwaredesign.orbi.model.real.google.ElevationResponse;
import foundation.softwaredesign.orbi.model.real.google.Location;
import foundation.softwaredesign.orbi.model.virtual.Coordinates;
import foundation.softwaredesign.orbi.model.virtual.Cube;
import foundation.softwaredesign.orbi.model.virtual.World;
import foundation.softwaredesign.orbi.persistence.repo.CubeRepository;
import foundation.softwaredesign.orbi.service.WorldAdapter;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.validation.constraints.NotNull;
import javax.ws.rs.*;
import javax.ws.rs.client.Client;
import javax.ws.rs.client.ClientBuilder;
import javax.ws.rs.core.MediaType;
import java.math.BigDecimal;
import java.util.List;

import static javax.ws.rs.core.MediaType.APPLICATION_JSON;
import static javax.ws.rs.core.MediaType.APPLICATION_XML;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Path("/api")
@Produces({APPLICATION_XML, APPLICATION_JSON})
@RequestScoped
public class RestApi {

    @Inject
    CubeRepository cubeRepository;

    @Inject
    WorldAdapter worldAdapter;


    @GET
    @Path("/init")
    @Produces(MediaType.TEXT_PLAIN)
    public String init() {
        cubeRepository.deleteAll();

        // schreibtisch
        Cube cube = new Cube();
        cube.setCoordinates(new Coordinates(new BigDecimal(15.555060), new BigDecimal(0),new BigDecimal(47.067640) ));
        cubeRepository.save(cube);

        // werft
        cube = new Cube();
        cube.setCoordinates(new Coordinates(new BigDecimal(15.555330),new BigDecimal(0),new BigDecimal(47.067640)) );
        cubeRepository.save(cube);

        // parkplatz rechts
        cube = new Cube();
        cube.setCoordinates(new Coordinates(new BigDecimal(15.555080),new BigDecimal(0),new BigDecimal(47.067500) ));
        cubeRepository.save(cube);

        // parkplatz rechts
        cube = new Cube();
        cube.setCoordinates(new Coordinates( new BigDecimal(15.555330) ,new BigDecimal(0),new BigDecimal(47.067500)));
        cubeRepository.save(cube);

        // schreibtisch
        //cube = new Cube();
        //cube.setCoordinates(new Coordinates(new BigDecimal(47.067551),new BigDecimal(0), new BigDecimal(15.555174)));
        //cubeRepository.save(cube);
        return "OK";
    }

    @GET
    @Path("/elevation")
    @Produces(MediaType.TEXT_PLAIN)
    public String elevation(@NotNull @QueryParam("user") String user,
                               @NotNull @QueryParam("longitude") BigDecimal longitude,
                               @NotNull @QueryParam("latitude") BigDecimal latitude) {
        Client client = ClientBuilder.newClient();

        String locationString =  new Location(latitude,longitude).toString();

        ElevationResponse elevationResponse = client.target("https://maps.googleapis.com/maps/api/elevation")
                .path("xml")
                .queryParam("locations",locationString)
                .queryParam("key", "AIzaSyDI8nlMT-eYC_-qg5Og0H1LB6FjxpUCCDg")
                .request(APPLICATION_XML)
                .get(ElevationResponse.class);

        String status = elevationResponse.getStatus();

        if (status.equals("OK")) {
            System.out.println(elevationResponse.getResult().get(0).getElevation());
        }
        return status;
    }

    @GET
    @Path("/world")
    public World plane(@NotNull @QueryParam("user") String user,
                       @NotNull @QueryParam("longitude") BigDecimal longitude,
                       @NotNull @QueryParam("latitude") BigDecimal latitude) {

        World world = new World();
        List<Cube> cubeList = cubeRepository.findAll();
        world.setCubes(cubeList);
        // TODO elevation
        Position position =  new Position(latitude,longitude,new BigDecimal(0.0));
        System.out.println("Requestion world at " + position);
        worldAdapter.convertToVirtual(world, position);
        return world;
    }

}
